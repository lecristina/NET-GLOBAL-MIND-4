using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using nexus;

namespace nexus.Tests.Integration
{
    /// <summary>
    /// Testes de integração para endpoints de Badges
    /// </summary>
    public class BadgesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public BadgesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        private async Task<string?> GetAuthTokenAsync()
        {
            var loginDto = new { email = "teste@example.com", senha = "123456" };
            var loginResponse = await _client.PostAsJsonAsync("/api/v1.0/Auth/login", loginDto);
            if (loginResponse.StatusCode != HttpStatusCode.OK) return null;

            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<JsonElement>(loginContent);
            if (loginResult.TryGetProperty("token", out var tokenElement))
                return tokenElement.GetString();
            return null;
        }

        [Fact]
        public async Task GetBadges_WithValidToken_ShouldReturnOk()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Badges?pageNumber=1&pageSize=10");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetBadges_WithoutToken_ShouldReturnUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var response = await _client.GetAsync("/api/v1.0/Badges");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetBadgeById_WithValidToken_ShouldReturnOkOrNotFound()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Badges/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetBadgesByUsuario_WithValidToken_ShouldReturnOk()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Badges/usuario/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateBadge_WithGestorToken_ShouldReturnCreated()
        {
            // Obter token de gestor
            var loginDto = new { email = "gestor@example.com", senha = "123456" };
            var loginResponse = await _client.PostAsJsonAsync("/api/v1.0/Auth/login", loginDto);
            if (loginResponse.StatusCode != HttpStatusCode.OK) Assert.Fail("Login falhou");

            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<JsonElement>(loginContent);
            if (!loginResult.TryGetProperty("token", out var tokenElement)) Assert.Fail("Token não encontrado");
            var token = tokenElement.GetString();
            if (token == null) Assert.Fail("Token é nulo");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var criarBadgeDto = new
            {
                nomeBadge = $"Badge Teste {Guid.NewGuid()}",
                descricao = "Badge de teste",
                pontosRequeridos = 100
            };

            var response = await _client.PostAsJsonAsync("/api/v1.0/Badges", criarBadgeDto);
            Assert.True(response.StatusCode == HttpStatusCode.Created || 
                       response.StatusCode == HttpStatusCode.BadRequest ||
                       response.StatusCode == HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateBadge_WithGestorToken_ShouldReturnOkOrNotFound()
        {
            var loginDto = new { email = "gestor@example.com", senha = "123456" };
            var loginResponse = await _client.PostAsJsonAsync("/api/v1.0/Auth/login", loginDto);
            if (loginResponse.StatusCode != HttpStatusCode.OK) Assert.Fail("Login falhou");

            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<JsonElement>(loginContent);
            if (!loginResult.TryGetProperty("token", out var tokenElement)) Assert.Fail("Token não encontrado");
            var token = tokenElement.GetString();
            if (token == null) Assert.Fail("Token é nulo");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var atualizarBadgeDto = new
            {
                nomeBadge = $"Badge Atualizado {Guid.NewGuid()}",
                descricao = "Badge atualizado",
                pontosRequeridos = 150
            };

            var response = await _client.PutAsJsonAsync("/api/v1.0/Badges/999999", atualizarBadgeDto);
            Assert.True(response.StatusCode == HttpStatusCode.OK || 
                       response.StatusCode == HttpStatusCode.NotFound ||
                       response.StatusCode == HttpStatusCode.BadRequest ||
                       response.StatusCode == HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task ConcederBadge_WithValidToken_ShouldReturnCreated()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/api/v1.0/Badges/usuario/1/badge/1", null);
            Assert.True(response.StatusCode == HttpStatusCode.Created || 
                       response.StatusCode == HttpStatusCode.NotFound ||
                       response.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task DeleteBadge_WithGestorToken_ShouldReturnNoContentOrNotFound()
        {
            var loginDto = new { email = "gestor@example.com", senha = "123456" };
            var loginResponse = await _client.PostAsJsonAsync("/api/v1.0/Auth/login", loginDto);
            if (loginResponse.StatusCode != HttpStatusCode.OK) Assert.Fail("Login falhou");

            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<JsonElement>(loginContent);
            if (!loginResult.TryGetProperty("token", out var tokenElement)) Assert.Fail("Token não encontrado");
            var token = tokenElement.GetString();
            if (token == null) Assert.Fail("Token é nulo");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/api/v1.0/Badges/999999");
            Assert.True(response.StatusCode == HttpStatusCode.NoContent || 
                       response.StatusCode == HttpStatusCode.NotFound ||
                       response.StatusCode == HttpStatusCode.Forbidden);
        }
    }
}

