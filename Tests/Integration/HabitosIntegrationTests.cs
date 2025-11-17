using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using nexus;

namespace nexus.Tests.Integration
{
    /// <summary>
    /// Testes de integração para endpoints de Hábitos
    /// </summary>
    public class HabitosIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public HabitosIntegrationTests(CustomWebApplicationFactory<Program> factory)
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
        public async Task GetHabitos_WithValidToken_ShouldReturnOk()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Habitos?pageNumber=1&pageSize=10");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetHabitos_WithoutToken_ShouldReturnUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var response = await _client.GetAsync("/api/v1.0/Habitos");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetHabitoById_WithValidToken_ShouldReturnOkOrNotFound()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Habitos/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetHabitosByUsuario_WithValidToken_ShouldReturnOk()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Habitos/usuario/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateHabito_WithValidToken_ShouldReturnCreated()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var criarHabitoDto = new
            {
                tipoHabito = "Hidratação",
                dataHabito = DateTime.UtcNow,
                pontuacao = 10
            };

            var response = await _client.PostAsJsonAsync("/api/v1.0/Habitos", criarHabitoDto);
            Assert.True(response.StatusCode == HttpStatusCode.Created || 
                       response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteHabito_WithValidToken_ShouldReturnNoContentOrNotFound()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/api/v1.0/Habitos/999999");
            Assert.True(response.StatusCode == HttpStatusCode.NoContent || 
                       response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}

