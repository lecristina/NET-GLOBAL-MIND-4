using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using challenge_3_net.Services.Auth;
using challenge_3_net.Models.DTOs;
using System.Security.Claims;

namespace challenge_3_net.Controllers
{
    /// <summary>
    /// Controller para autenticação e autorização
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1")] // Aceitar também v1
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(JwtService jwtService, ILogger<AuthController> logger)
        {
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// Autentica um usuário e retorna um token JWT
        /// </summary>
        /// <param name="loginDto">Dados de login do usuário</param>
        /// <returns>Token JWT se autenticação bem-sucedida</returns>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="401">Credenciais inválidas</response>
        /// <response code="400">Dados de entrada inválidos</response>
        [HttpPost("login")]
        [AllowAnonymous] // Login não precisa de autenticação
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto? loginDto)
        {
            try
            {
                // Verificar se o DTO foi recebido
                if (loginDto == null)
                {
                    return BadRequest(new ErrorResponseDto
                    {
                        Message = "Dados de entrada inválidos",
                        Details = new[] { "O corpo da requisição deve conter um objeto JSON válido com 'email' e 'senha'" }
                    });
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                        .ToList();

                    return BadRequest(new ErrorResponseDto
                    {
                        Message = "Dados de entrada inválidos",
                        Details = errors
                    });
                }

                _logger.LogInformation("Tentativa de login para email: {Email}", loginDto.Email);

                var token = await _jwtService.AuthenticateAsync(loginDto.Email, loginDto.Senha);

                if (token == null)
                {
                    _logger.LogWarning("Falha na autenticação para email: {Email}", loginDto.Email);
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = "Email ou senha incorretos",
                        Details = new[] { "Verifique suas credenciais e tente novamente" }
                    });
                }

                _logger.LogInformation("Login realizado com sucesso para email: {Email}", loginDto.Email);

                return Ok(new LoginResponseDto
                {
                    Token = token,
                    TokenType = "Bearer",
                    ExpiresIn = 3600, // 1 hora
                    Message = "Login realizado com sucesso"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante processo de login para email: {Email}", loginDto?.Email ?? "N/A");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = "Erro interno do servidor",
                    Details = new[] { "Tente novamente mais tarde" }
                });
            }
        }

        /// <summary>
        /// Valida um token JWT e retorna informações do usuário
        /// </summary>
        /// <returns>Informações do usuário autenticado</returns>
        /// <response code="200">Token válido</response>
        /// <response code="401">Token inválido ou expirado</response>
        [HttpPost("validate")]
        [AllowAnonymous] // Permitir validação manual do token
        [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ValidateToken()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authHeader))
                {
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = "Token de autorização não fornecido",
                        Details = new[] { "Inclua o header 'Authorization: Bearer {token}'" }
                    });
                }

                // Extrair token (pode vir com ou sem "Bearer ")
                var token = authHeader;
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                else if (token.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase) && token.Length > 6)
                {
                    token = token.Substring(6).Trim();
                }
                else
                {
                    token = token.Trim();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = "Token de autorização inválido",
                        Details = new[] { "O token não pode estar vazio" }
                    });
                }
                var principal = _jwtService.ValidateToken(token);

                if (principal == null)
                {
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = "Token inválido ou expirado",
                        Details = new[] { "Faça login novamente para obter um novo token" }
                    });
                }

                var usuario = await _jwtService.GetCurrentUserAsync(principal);
                if (usuario == null)
                {
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = "Usuário não encontrado",
                        Details = new[] { "Token válido mas usuário não existe mais" }
                    });
                }

                return Ok(new UserInfoDto
                {
                    Id = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Perfil = usuario.Perfil,
                    Empresa = usuario.Empresa,
                    DataCadastro = usuario.DataCadastro,
                    Roles = principal.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante validação de token");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = "Erro interno do servidor",
                    Details = new[] { "Tente novamente mais tarde" }
                });
            }
        }

        /// <summary>
        /// Obtém informações do usuário atual
        /// </summary>
        /// <returns>Informações do usuário atual</returns>
        /// <response code="200">Informações obtidas com sucesso</response>
        /// <response code="401">Usuário não autenticado</response>
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var usuario = await _jwtService.GetCurrentUserAsync(User);
                if (usuario == null)
                {
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = "Usuário não autenticado",
                        Details = new[] { "Faça login para acessar este recurso" }
                    });
                }

                return Ok(new UserInfoDto
                {
                    Id = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Perfil = usuario.Perfil,
                    Empresa = usuario.Empresa,
                    DataCadastro = usuario.DataCadastro,
                    Roles = User.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter informações do usuário atual");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = "Erro interno do servidor",
                    Details = new[] { "Tente novamente mais tarde" }
                });
            }
        }

        /// <summary>
        /// Verifica se o usuário atual tem permissões de administrador
        /// </summary>
        /// <returns>Status de permissão</returns>
        /// <response code="200">Verificação realizada</response>
        /// <response code="401">Usuário não autenticado</response>
        [HttpGet("check-admin")]
        [ProducesResponseType(typeof(PermissionCheckDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public IActionResult CheckAdminPermission()
        {
            try
            {
                var isGestor = _jwtService.IsGestor(User);
                var isGestorOrProfissional = _jwtService.IsGestorOrProfissional(User);

                return Ok(new PermissionCheckDto
                {
                    IsAdmin = isGestor,
                    IsManager = isGestorOrProfissional,
                    IsOperador = User.IsInRole("PROFISSIONAL"),
                    Message = isGestor ? "Usuário tem permissões de gestor" : 
                              isGestorOrProfissional ? "Usuário tem permissões de profissional" : 
                              "Usuário tem permissões limitadas"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar permissões do usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
                {
                    Message = "Erro interno do servidor",
                    Details = new[] { "Tente novamente mais tarde" }
                });
            }
        }
    }

    /// <summary>
    /// DTO para resposta de login
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Token JWT
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do token
        /// </summary>
        public string TokenType { get; set; } = string.Empty;

        /// <summary>
        /// Tempo de expiração em segundos
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Mensagem de resposta
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para informações do usuário
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Perfil do usuário
        /// </summary>
        public string Perfil { get; set; } = string.Empty;

        /// <summary>
        /// Empresa do usuário
        /// </summary>
        public string? Empresa { get; set; }

        /// <summary>
        /// Data de cadastro
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Roles do usuário
        /// </summary>
        public List<string> Roles { get; set; } = new();
    }

    /// <summary>
    /// DTO para verificação de permissões
    /// </summary>
    public class PermissionCheckDto
    {
        /// <summary>
        /// Se é administrador
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Se é gerente ou administrador
        /// </summary>
        public bool IsManager { get; set; }

        /// <summary>
        /// Se é operador
        /// </summary>
        public bool IsOperador { get; set; }

        /// <summary>
        /// Mensagem descritiva
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para resposta de erro
    /// </summary>
    public class ErrorResponseDto
    {
        /// <summary>
        /// Mensagem de erro
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detalhes do erro
        /// </summary>
        public IEnumerable<string> Details { get; set; } = new List<string>();
    }
}
