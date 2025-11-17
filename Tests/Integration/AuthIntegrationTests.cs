using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using challenge_3_net;

namespace challenge_3_net.Tests.Integration
{
    /// <summary>
    /// Testes de integração para autenticação
    /// </summary>
    public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new
            {
                email = "teste@example.com",
                senha = "123456"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1.0/Auth/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("token", content);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginDto = new
            {
                email = "invalid@example.com",
                senha = "wrongpassword"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1.0/Auth/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ValidateToken_WithValidToken_ShouldReturnValid()
        {
            // Arrange - Primeiro fazer login para obter token
            var loginDto = new
            {
                email = "teste@example.com",
                senha = "123456"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/v1.0/Auth/login", loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<JsonElement>(loginContent);
            
            if (!loginResult.TryGetProperty("token", out var tokenElement))
            {
                Assert.Fail("Token não encontrado na resposta de login");
                return;
            }

            var token = tokenElement.GetString();
            if (token == null)
            {
                Assert.Fail("Token é nulo");
                return;
            }

            // Act - O endpoint validate é POST, não GET
            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var validateResponse = await _client.PostAsync("/api/v1.0/Auth/validate", null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, validateResponse.StatusCode);
        }

        [Fact]
        public async Task GetUserInfo_WithValidToken_ShouldReturnUserInfo()
        {
            // Arrange - Primeiro fazer login para obter token
            var loginDto = new
            {
                email = "teste@example.com",
                senha = "123456"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/v1.0/Auth/login", loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<JsonElement>(loginContent);
            
            if (!loginResult.TryGetProperty("token", out var tokenElement))
            {
                Assert.Fail("Token não encontrado na resposta de login");
                return;
            }

            var token = tokenElement.GetString();
            if (token == null)
            {
                Assert.Fail("Token é nulo");
                return;
            }

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Act - O endpoint correto é /me, não /user-info
            var response = await _client.GetAsync("/api/v1.0/Auth/me");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("email", content);
        }

        [Fact]
        public async Task GetUserInfo_WithoutToken_ShouldReturnUnauthorized()
        {
            // Arrange - Não adicionar token
            _client.DefaultRequestHeaders.Authorization = null;

            // Act - O endpoint correto é /me, não /user-info
            var response = await _client.GetAsync("/api/v1.0/Auth/me");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CheckAdmin_WithValidToken_ShouldReturnOk()
        {
            // Arrange - Primeiro fazer login para obter token
            var loginDto = new
            {
                email = "teste@example.com",
                senha = "123456"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/v1.0/Auth/login", loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<JsonElement>(loginContent);
            
            if (!loginResult.TryGetProperty("token", out var tokenElement))
            {
                Assert.Fail("Token não encontrado na resposta de login");
                return;
            }

            var token = tokenElement.GetString();
            if (token == null)
            {
                Assert.Fail("Token é nulo");
                return;
            }

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/v1.0/Auth/check-admin");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("isAdmin", content);
        }

        [Fact]
        public async Task CheckAdmin_WithoutToken_ShouldReturnOk()
        {
            // Arrange - Não adicionar token
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.GetAsync("/api/v1.0/Auth/check-admin");

            // Assert - O endpoint não requer autenticação, retorna OK mesmo sem token
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("isAdmin", content);
        }
    }
}

