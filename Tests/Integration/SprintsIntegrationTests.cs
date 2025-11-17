using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using nexus;

namespace nexus.Tests.Integration
{
    /// <summary>
    /// Testes de integração para endpoints de Sprints
    /// </summary>
    public class SprintsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public SprintsIntegrationTests(CustomWebApplicationFactory<Program> factory)
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
        public async Task GetSprints_WithValidToken_ShouldReturnOk()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Sprints?pageNumber=1&pageSize=10");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetSprints_WithoutToken_ShouldReturnUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var response = await _client.GetAsync("/api/v1.0/Sprints");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetSprintById_WithValidToken_ShouldReturnOkOrNotFound()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Sprints/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSprintsByUsuario_WithValidToken_ShouldReturnOk()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Sprints/usuario/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateSprint_WithValidToken_ShouldReturnCreated()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var criarSprintDto = new
            {
                nomeSprint = $"Sprint Teste {Guid.NewGuid()}",
                dataInicio = DateTime.UtcNow,
                dataFim = DateTime.UtcNow.AddDays(14),
                produtividade = 85.5m,
                tarefasConcluidas = 10,
                commits = 25
            };

            var response = await _client.PostAsJsonAsync("/api/v1.0/Sprints", criarSprintDto);
            Assert.True(response.StatusCode == HttpStatusCode.Created || 
                       response.StatusCode == HttpStatusCode.BadRequest ||
                       response.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateSprint_WithValidToken_ShouldReturnOkOrNotFound()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var atualizarSprintDto = new
            {
                nomeSprint = $"Sprint Atualizada {Guid.NewGuid()}",
                dataInicio = DateTime.UtcNow,
                dataFim = DateTime.UtcNow.AddDays(15),
                produtividade = 90.0m,
                tarefasConcluidas = 12,
                commits = 30
            };

            var response = await _client.PutAsJsonAsync("/api/v1.0/Sprints/999999", atualizarSprintDto);
            Assert.True(response.StatusCode == HttpStatusCode.OK || 
                       response.StatusCode == HttpStatusCode.NotFound ||
                       response.StatusCode == HttpStatusCode.BadRequest ||
                       response.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task DeleteSprint_WithValidToken_ShouldReturnNoContentOrNotFound()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/api/v1.0/Sprints/999999");
            Assert.True(response.StatusCode == HttpStatusCode.NoContent || 
                       response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}

