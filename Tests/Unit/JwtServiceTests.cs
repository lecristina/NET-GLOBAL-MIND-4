using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nexus.Services.Auth;
using nexus.Models;
using nexus.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace nexus.Tests.Unit
{
    /// <summary>
    /// Testes unitários para JwtService
    /// </summary>
    public class JwtServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<JwtService>> _mockLogger;
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public JwtServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<JwtService>>();

            // Configurar mock do IConfiguration com seção JwtSettings
            var jwtSettingsSection = new Mock<IConfigurationSection>();
            jwtSettingsSection.Setup(x => x["SecretKey"])
                .Returns("TestSecretKeyForUnitTesting123456789");
            jwtSettingsSection.Setup(x => x["Issuer"])
                .Returns("TestIssuer");
            jwtSettingsSection.Setup(x => x["Audience"])
                .Returns("TestAudience");
            jwtSettingsSection.Setup(x => x["ExpiryMinutes"])
                .Returns("60");

            _mockConfiguration.Setup(x => x.GetSection("JwtSettings"))
                .Returns(jwtSettingsSection.Object);

            // Configurar DbContext em memória
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            _jwtService = new JwtService(_mockConfiguration.Object, _context, _mockLogger.Object);
        }

        [Fact]
        public void GenerateToken_WithValidUsuario_ShouldReturnValidToken()
        {
            // Arrange
            var usuario = new Usuario
            {
                IdUsuario = 1,
                Nome = "Usuário Teste",
                Email = "teste@teste.com",
                Perfil = "GESTOR",
                Empresa = "Empresa Teste",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                DataCadastro = DateTime.UtcNow
            };

            // Act
            var token = _jwtService.GenerateToken(usuario);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            Assert.Contains(".", token); // JWT deve ter pontos separadores
        }

        [Fact]
        public void GenerateToken_WithDifferentPerfis_ShouldGenerateDifferentTokens()
        {
            // Arrange
            var gestorUsuario = new Usuario
            {
                IdUsuario = 1,
                Nome = "Gestor Teste",
                Email = "gestor@teste.com",
                Perfil = "GESTOR",
                Empresa = "Empresa Teste",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                DataCadastro = DateTime.UtcNow
            };

            var profissionalUsuario = new Usuario
            {
                IdUsuario = 2,
                Nome = "Profissional Teste",
                Email = "profissional@teste.com",
                Perfil = "PROFISSIONAL",
                Empresa = "Empresa Teste",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                DataCadastro = DateTime.UtcNow
            };

            // Act
            var gestorToken = _jwtService.GenerateToken(gestorUsuario);
            var profissionalToken = _jwtService.GenerateToken(profissionalUsuario);

            // Assert
            Assert.NotEqual(gestorToken, profissionalToken);
        }

        [Fact]
        public void ValidateToken_WithValidToken_ShouldReturnClaimsPrincipal()
        {
            // Arrange
            var usuario = new Usuario
            {
                IdUsuario = 1,
                Nome = "Usuário Teste",
                Email = "teste@teste.com",
                Perfil = "GESTOR",
                Empresa = "Empresa Teste",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                DataCadastro = DateTime.UtcNow
            };

            var token = _jwtService.GenerateToken(usuario);

            // Act
            var principal = _jwtService.ValidateToken(token);

            // Assert
            Assert.NotNull(principal);
            Assert.NotNull(principal.Identity);
            Assert.True(principal.Identity.IsAuthenticated);
            Assert.Equal("1", principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal("Usuário Teste", principal.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value);
            Assert.Equal("teste@teste.com", principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value);
        }

        [Fact]
        public void ValidateToken_WithInvalidToken_ShouldReturnNull()
        {
            // Arrange
            var invalidToken = "invalid.token.here";

            // Act
            var principal = _jwtService.ValidateToken(invalidToken);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void ValidateToken_WithExpiredToken_ShouldReturnNull()
        {
            // Arrange
            var expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjF9.invalid";

            // Act
            var principal = _jwtService.ValidateToken(expiredToken);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void HasRole_WithGestorUser_ShouldReturnTrueForGestorRole()
        {
            // Arrange
            var usuario = new Usuario
            {
                IdUsuario = 1,
                Nome = "Gestor Teste",
                Email = "gestor@teste.com",
                Perfil = "GESTOR",
                Empresa = "Empresa Teste",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                DataCadastro = DateTime.UtcNow
            };

            var token = _jwtService.GenerateToken(usuario);
            var principal = _jwtService.ValidateToken(token);

            // Act
            Assert.NotNull(principal);
            var hasGestorRole = _jwtService.HasRole(principal, "GESTOR");
            var isGestor = _jwtService.IsGestor(principal);
            var isGestorOrProfissional = _jwtService.IsGestorOrProfissional(principal);

            // Assert
            Assert.True(hasGestorRole);
            Assert.True(isGestor);
            Assert.True(isGestorOrProfissional);
        }

        [Fact]
        public void HasRole_WithProfissionalUser_ShouldReturnFalseForGestorRole()
        {
            // Arrange
            var usuario = new Usuario
            {
                IdUsuario = 1,
                Nome = "Profissional Teste",
                Email = "profissional@teste.com",
                Perfil = "PROFISSIONAL",
                Empresa = "Empresa Teste",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                DataCadastro = DateTime.UtcNow
            };

            var token = _jwtService.GenerateToken(usuario);
            var principal = _jwtService.ValidateToken(token);

            // Act
            Assert.NotNull(principal);
            var hasGestorRole = _jwtService.HasRole(principal, "GESTOR");
            var isGestor = _jwtService.IsGestor(principal);
            var hasProfissionalRole = _jwtService.HasRole(principal, "PROFISSIONAL");

            // Assert
            Assert.False(hasGestorRole);
            Assert.False(isGestor);
            Assert.True(hasProfissionalRole);
        }

        private void Dispose()
        {
            _context.Dispose();
        }
    }
}
