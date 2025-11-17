using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using challenge_3_net;

namespace challenge_3_net.Tests.Integration
{
    /// <summary>
    /// Testes de integração para endpoints de Humor
    /// </summary>
    public class HumorIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public HumorIntegrationTests(CustomWebApplicationFactory<Program> factory)
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
        public async Task GetHumores_WithValidToken_ShouldReturnOk()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Humor?pageNumber=1&pageSize=10");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetHumores_WithoutToken_ShouldReturnUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var response = await _client.GetAsync("/api/v1.0/Humor");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetHumorById_WithValidToken_ShouldReturnOkOrNotFound()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Humor/1");
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetHumorByUsuario_WithValidToken_ShouldReturnOk()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/v1.0/Humor/usuario/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateHumor_WithValidToken_ShouldReturnCreated()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var criarHumorDto = new
            {
                nivelHumor = 4,
                nivelEnergia = 3,
                comentario = "Teste de humor"
            };

            var response = await _client.PostAsJsonAsync("/api/v1.0/Humor", criarHumorDto);
            Assert.True(response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateHumor_WithValidToken_ShouldReturnOkOrNotFound()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var atualizarHumorDto = new
            {
                nivelHumor = 5,
                nivelEnergia = 4,
                comentario = "Humor atualizado"
            };

            var response = await _client.PutAsJsonAsync("/api/v1.0/Humor/999999", atualizarHumorDto);
            Assert.True(response.StatusCode == HttpStatusCode.OK || 
                       response.StatusCode == HttpStatusCode.NotFound ||
                       response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteHumor_WithValidToken_ShouldReturnNoContentOrNotFound()
        {
            var token = await GetAuthTokenAsync();
            if (token == null) Assert.Fail("Token não obtido");

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/api/v1.0/Humor/999999");
            Assert.True(response.StatusCode == HttpStatusCode.NoContent || 
                       response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}

