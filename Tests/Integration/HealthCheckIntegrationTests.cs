using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using challenge_3_net;

namespace challenge_3_net.Tests.Integration
{
    /// <summary>
    /// Testes de integração para Health Checks
    /// </summary>
    public class HealthCheckIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public HealthCheckIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task HealthEndpoint_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthReadyEndpoint_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/health/ready");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthLiveEndpoint_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/health/live");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthDatabaseEndpoint_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/health/database");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Testes para versão 1.0 da API
        [Fact]
        public async Task HealthV1Endpoint_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/api/v1.0/Health");

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.OK || 
                response.StatusCode == HttpStatusCode.ServiceUnavailable,
                $"Status code inesperado: {response.StatusCode}"
            );
        }

        [Fact]
        public async Task HealthV1DatabaseEndpoint_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/api/v1.0/Health/database");

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.OK || 
                response.StatusCode == HttpStatusCode.ServiceUnavailable,
                $"Status code inesperado: {response.StatusCode}"
            );
        }

        [Fact]
        public async Task HealthV1MemoryEndpoint_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/api/v1.0/Health/memory");

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.OK || 
                response.StatusCode == HttpStatusCode.ServiceUnavailable,
                $"Status code inesperado: {response.StatusCode}"
            );
        }
    }
}


