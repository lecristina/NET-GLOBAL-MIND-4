using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using challenge_3_net.Data;
using System;
using System.Linq;
using BCrypt.Net;

namespace challenge_3_net.Tests.Integration
{
    /// <summary>
    /// Factory customizada para testes de integração que usa banco de dados em memória
    /// </summary>
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Configurar o ambiente ANTES de configurar os serviços
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remover o DbContextOptions<ApplicationDbContext> registrado pelo Program.cs
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Substituir completamente o DbContext com banco de dados em memória
                // Cada instância da factory terá seu próprio banco isolado
                var databaseName = $"TestDb_{Guid.NewGuid()}";
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    // IMPORTANTE: Limpar qualquer configuração anterior e usar apenas InMemory
                    options.UseInMemoryDatabase(databaseName);
                    // Desabilitar rastreamento para melhor performance em testes
                    options.EnableSensitiveDataLogging(false);
                }, ServiceLifetime.Scoped);

                // Garantir que o banco seja criado e populado com dados de teste
                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                    // Garantir que o banco seja criado
                    db.Database.EnsureCreated();

                    // Popular com dados de teste básicos
                    SeedTestData(db);
                }
            });
        }

        /// <summary>
        /// Popula o banco de dados em memória com dados de teste
        /// </summary>
        private void SeedTestData(ApplicationDbContext context)
        {
            // Limpar dados existentes
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Criar usuário de teste para autenticação
            if (!context.Usuarios.Any(u => u.Email == "teste@example.com"))
            {
                var usuario = new challenge_3_net.Models.Usuario
                {
                    Nome = "Usuário Teste",
                    Email = "teste@example.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Perfil = "PROFISSIONAL",
                    Empresa = "Empresa Teste",
                    DataCadastro = DateTime.UtcNow
                };
                context.Usuarios.Add(usuario);
                context.SaveChanges(); // Salvar para obter o ID
            }

            // Criar usuário gestor de teste
            if (!context.Usuarios.Any(u => u.Email == "gestor@example.com"))
            {
                var gestor = new challenge_3_net.Models.Usuario
                {
                    Nome = "Gestor Teste",
                    Email = "gestor@example.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Perfil = "GESTOR",
                    Empresa = "Empresa Teste",
                    DataCadastro = DateTime.UtcNow
                };
                context.Usuarios.Add(gestor);
                context.SaveChanges();
            }
        }
    }
}
