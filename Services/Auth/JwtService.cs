using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using challenge_3_net.Models;
using challenge_3_net.Data;
using Microsoft.EntityFrameworkCore;

namespace challenge_3_net.Services.Auth
{
    /// <summary>
    /// Serviço para geração e validação de tokens JWT
    /// </summary>
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IConfiguration configuration, ApplicationDbContext context, ILogger<JwtService> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Gera um token JWT para o usuário
        /// </summary>
        /// <param name="usuario">Usuário para o qual gerar o token</param>
        /// <returns>Token JWT</returns>
        public string GenerateToken(Usuario usuario)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"] ?? "MindTrack_Super_Secret_Key_2024_Advanced_Business_Development_With_DotNet";
                var issuer = jwtSettings["Issuer"] ?? "MindTrackAPI";
                var audience = jwtSettings["Audience"] ?? "MindTrackUsers";
                var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim("perfil", usuario.Perfil),
                    new Claim("empresa", usuario.Empresa ?? ""),
                    new Claim("jti", Guid.NewGuid().ToString()),
                    new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };

                // Adicionar roles baseadas no perfil
                if (usuario.Perfil == "PROFISSIONAL")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "PROFISSIONAL"));
                }
                else if (usuario.Perfil == "GESTOR")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "GESTOR"));
                    claims.Add(new Claim(ClaimTypes.Role, "PROFISSIONAL"));
                }

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                
                _logger.LogInformation("Token JWT gerado com sucesso para usuário {UsuarioId}", usuario.IdUsuario);
                
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar token JWT para usuário {UsuarioId}", usuario.IdUsuario);
                throw new InvalidOperationException("Erro interno ao gerar token de autenticação", ex);
            }
        }

        /// <summary>
        /// Valida um token JWT
        /// </summary>
        /// <param name="token">Token a ser validado</param>
        /// <returns>Claims do token se válido, null se inválido</returns>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"] ?? "MindTrack_Super_Secret_Key_2024_Advanced_Business_Development_With_DotNet";
                var issuer = jwtSettings["Issuer"] ?? "MindTrackAPI";
                var audience = jwtSettings["Audience"] ?? "MindTrackUsers";

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                
                _logger.LogInformation("Token JWT validado com sucesso");
                
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token JWT inválido ou expirado");
                return null;
            }
        }

        /// <summary>
        /// Autentica um usuário e retorna o token JWT
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        /// <returns>Token JWT se autenticação bem-sucedida, null caso contrário</returns>
        public async Task<string?> AuthenticateAsync(string email, string senha)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (usuario == null)
                {
                    _logger.LogWarning("Tentativa de login com email inexistente: {Email}", email);
                    return null;
                }

                // Verificar senha (assumindo que está usando BCrypt)
                if (!BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
                {
                    _logger.LogWarning("Tentativa de login com senha incorreta para usuário {Email}", email);
                    return null;
                }

                var token = GenerateToken(usuario);
                
                _logger.LogInformation("Usuário {Email} autenticado com sucesso", email);
                
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante autenticação do usuário {Email}", email);
                return null;
            }
        }

        /// <summary>
        /// Obtém o usuário atual a partir do token JWT
        /// </summary>
        /// <param name="principal">Claims principal do usuário autenticado</param>
        /// <returns>Usuário atual ou null se não encontrado</returns>
        public async Task<Usuario?> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            try
            {
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return null;
                }

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.IdUsuario == userId);

                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter usuário atual");
                return null;
            }
        }

        /// <summary>
        /// Verifica se o usuário tem uma role específica
        /// </summary>
        /// <param name="principal">Claims principal do usuário</param>
        /// <param name="role">Role a ser verificada</param>
        /// <returns>True se o usuário tem a role, false caso contrário</returns>
        public bool HasRole(ClaimsPrincipal principal, string role)
        {
            return principal.IsInRole(role);
        }

        /// <summary>
        /// Verifica se o usuário tem permissão de administrador
        /// </summary>
        /// <param name="principal">Claims principal do usuário</param>
        /// <returns>True se é admin, false caso contrário</returns>
        public bool IsGestor(ClaimsPrincipal principal)
        {
            return HasRole(principal, "GESTOR");
        }

        /// <summary>
        /// Verifica se o usuário tem permissão de gestor ou profissional
        /// </summary>
        /// <param name="principal">Claims principal do usuário</param>
        /// <returns>True se é gestor ou profissional, false caso contrário</returns>
        public bool IsGestorOrProfissional(ClaimsPrincipal principal)
        {
            return HasRole(principal, "GESTOR") || HasRole(principal, "PROFISSIONAL");
        }
    }
}
