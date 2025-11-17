using Microsoft.EntityFrameworkCore;
using challenge_3_net.Data;
using challenge_3_net.Repositories;
using challenge_3_net.Repositories.Interfaces;
using challenge_3_net.Services;
using challenge_3_net.Services.Interfaces;
using challenge_3_net.Services.Mapping;
using challenge_3_net.Services.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar versionamento da API (opcional - assume v1.0 se não especificado)
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"),
        new UrlSegmentApiVersionReader() // Permite /api/v1.0/ ou /api/v1/ ou /api/ (sem versão)
    );
    opt.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Configurar Application Insights
builder.Services.AddApplicationInsightsTelemetry();

// Configurar Entity Framework
// Em ambiente de teste, o CustomWebApplicationFactory irá substituir esta configuração
if (!builder.Environment.IsEnvironment("Testing"))
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        if (!string.IsNullOrEmpty(connectionString))
        {
            options.UseOracle(connectionString);
        }
        else
        {
            // Fallback para Oracle FIAP em desenvolvimento
            options.UseOracle("Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm555241;Password=230205;Connection Timeout=30;");
        }
    });
}
else
{
    // Em ambiente de teste, apenas registrar o DbContext sem provider
    // O CustomWebApplicationFactory irá substituir com InMemory
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        // Placeholder - será substituído pelo CustomWebApplicationFactory
    });
}

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Configurar para versionamento
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MindTrack API",
        Version = "v1.0",
        Description = "API RESTful para monitoramento de bem-estar emocional e produtividade de profissionais de TI - Versão 1.0 (com ML.NET e JWT)",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Equipe de Desenvolvimento",
            Email = "dev@fiap.com"
        }
    });

    // Incluir comentários XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Configurar JWT no Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Configurar exemplos de payloads
    // c.EnableAnnotations(); // Comentado pois não está disponível na versão atual
});

// Configurar injeção de dependência - Repositórios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IHumorRepository, HumorRepository>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<IAlertaIARepository, AlertaIARepository>();
builder.Services.AddScoped<IHabitoRepository, HabitoRepository>();
builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();

// Configurar injeção de dependência - Serviços
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IHumorService, HumorService>();
builder.Services.AddScoped<ISprintService, SprintService>();
builder.Services.AddScoped<IAlertaIAService, AlertaIAService>();
builder.Services.AddScoped<IHabitoService, HabitoService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();

// Configurar serviços de autenticação
builder.Services.AddScoped<challenge_3_net.Services.Auth.JwtService>();

// Configurar serviços de ML
// TODO: Implementar serviço de ML para análise de bem-estar do MindTrack

// ============================================
// CONFIGURAÇÃO JWT - RECRIADA DO ZERO
// ============================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "MindTrack_Super_Secret_Key_2024_Advanced_Business_Development_With_DotNet";
var issuer = jwtSettings["Issuer"] ?? "MindTrackAPI";
var audience = jwtSettings["Audience"] ?? "MindTrackUsers";

// Configurar autenticação JWT - RECRIADO DO ZERO
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
    };
    
    // Eventos para debug
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            
            if (!string.IsNullOrEmpty(authHeader))
            {
                // Extrair token do header "Bearer {token}"
                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    context.Token = authHeader.Substring("Bearer ".Length).Trim();
                }
                else if (authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase) && authHeader.Length > 6)
                {
                    context.Token = authHeader.Substring(6).Trim();
                }
                else
                {
                    context.Token = authHeader.Trim();
                }
                
                logger.LogInformation("📥 OnMessageReceived - Token extraído: {Token}", 
                    !string.IsNullOrEmpty(context.Token) ? context.Token.Substring(0, Math.Min(30, context.Token.Length)) + "..." : "VAZIO");
            }
            else
            {
                logger.LogWarning("⚠️ OnMessageReceived - Header Authorization vazio");
            }
            
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError("❌ AUTH FAILED: {Error}", context.Exception.Message);
            logger.LogError("❌ Type: {Type}", context.Exception.GetType().Name);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var userId = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            logger.LogInformation("✅✅✅ TOKEN VALIDATED - UserId: {UserId}", userId);
            return Task.CompletedTask;
        }
    };
});

// Configurar autorização com políticas
builder.Services.AddAuthorization(options =>
{
    // Políticas de roles
    options.AddPolicy("Profissional", policy => policy.RequireRole("PROFISSIONAL", "GESTOR"));
    options.AddPolicy("Gestor", policy => policy.RequireRole("GESTOR"));
});

// Configurar HttpContextAccessor para HATEOAS
builder.Services.AddHttpContextAccessor();

// Configurar Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "database" })
    .AddCheck<MemoryHealthCheck>("memory", tags: new[] { "memory" });

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Log de inicialização
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=== Iniciando aplicação ===");
logger.LogInformation("Ambiente: {Environment}", app.Environment.EnvironmentName);
logger.LogInformation("Connection String presente: {HasConnectionString}", 
    !string.IsNullOrEmpty(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure the HTTP request pipeline.
// Habilitar Swagger em todos os ambientes para facilitar testes
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MindTrack API v1.0");
    if (app.Environment.IsDevelopment())
    {
        c.RoutePrefix = string.Empty; // Para acessar o Swagger na raiz apenas em dev
    }
    c.DocumentTitle = "MindTrack API - Versão 1.0";
    c.DisplayRequestDuration();
    c.EnableDeepLinking();
    c.EnableFilter();
    c.ShowExtensions();
    c.EnableValidator();
    c.EnableTryItOutByDefault();
    // Configurar o esquema de autorização padrão
    c.DefaultModelsExpandDepth(-1);
    c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

// Middleware de debug ANTES da autenticação
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
    logger.LogInformation("🔍 REQUEST: {Method} {Path}", context.Request.Method, context.Request.Path);
    logger.LogInformation("🔍 Authorization Header: {Header}", 
        !string.IsNullOrEmpty(authHeader) ? authHeader.Substring(0, Math.Min(50, authHeader.Length)) + "..." : "VAZIO");
    await next();
    
    if (context.Response.StatusCode == 401)
    {
        logger.LogError("❌ 401 RETORNADO - User autenticado: {IsAuth}", 
            context.User?.Identity?.IsAuthenticated ?? false);
    }
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Configurar Health Checks endpoints
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("database")
});

app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});

// Endpoint de health check detalhado
app.MapGet("/health", (IConfiguration config) => {
    try 
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        
        return Results.Ok(new { 
            Status = "Healthy", 
            Timestamp = DateTime.UtcNow,
            Environment = app.Environment.EnvironmentName,
            HasConnectionString = !string.IsNullOrEmpty(connectionString),
            ConnectionStringLength = connectionString?.Length ?? 0,
            EnvironmentVariables = new {
                DB_SERVER = Environment.GetEnvironmentVariable("DB_SERVER"),
                DB_DATABASE = Environment.GetEnvironmentVariable("DB_DATABASE"),
                DB_USERNAME = Environment.GetEnvironmentVariable("DB_USERNAME"),
                HasPassword = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_PASSWORD")),
                ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            }
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { 
            Status = "Error", 
            Error = ex.Message,
            Timestamp = DateTime.UtcNow 
        });
    }
});

// Endpoint de diagnóstico do banco
app.MapGet("/health/database", async (ApplicationDbContext context) => {
    try 
    {
        var canConnect = await context.Database.CanConnectAsync();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
        
        return Results.Ok(new { 
            Status = canConnect ? "Connected" : "Cannot Connect",
            CanConnect = canConnect,
            PendingMigrations = pendingMigrations,
            AppliedMigrations = appliedMigrations,
            Timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { 
            Status = "Database Error", 
            Error = ex.Message,
            InnerError = ex.InnerException?.Message,
            Timestamp = DateTime.UtcNow 
        });
    }
});

// Endpoint para verificar dados no banco
app.MapGet("/admin/data", async (ApplicationDbContext context) => {
    try 
    {
        var usuarios = await context.Usuarios.CountAsync();
        var humores = await context.Humores.CountAsync();
        var sprints = await context.Sprints.CountAsync();
        var alertasIA = await context.AlertasIA.CountAsync();
        var habitos = await context.Habitos.CountAsync();
        var badges = await context.Badges.CountAsync();
        
        return Results.Ok(new { 
            Status = "Success",
            DataCounts = new {
                Usuarios = usuarios,
                Humores = humores,
                Sprints = sprints,
                AlertasIA = alertasIA,
                Habitos = habitos,
                Badges = badges
            },
            Timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { 
            Status = "Error", 
            Error = ex.Message,
            InnerError = ex.InnerException?.Message,
            Timestamp = DateTime.UtcNow 
        });
    }
});

// Configurar banco de dados com tratamento de erro
var dbLogger = app.Services.GetRequiredService<ILogger<Program>>();
try
{
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Verificar se o banco está acessível (as tabelas já foram criadas pelo script SQL)
            var canConnect = await context.Database.CanConnectAsync();
            if (canConnect)
            {
                dbLogger.LogInformation("Banco de dados MindTrack conectado com sucesso!");
                
                // Verificar se existem dados de teste
                var usuarioCount = await context.Usuarios.CountAsync();
                dbLogger.LogInformation("Usuários encontrados no banco: {Count}", usuarioCount);
            }
            else
            {
                dbLogger.LogWarning("Não foi possível conectar ao banco de dados.");
            }
        }
}
catch (Exception ex)
{
    dbLogger.LogError(ex, "Erro ao configurar o banco de dados durante a inicialização");
    // Não parar a aplicação por causa do banco - deixar continuar e tratar nos endpoints
}

// Endpoint para testar diretamente os dados dos usuários
app.MapGet("/debug/usuarios", async (ApplicationDbContext context) => {
    try 
    {
        var usuarios = await context.Usuarios.Take(5).ToListAsync();
        return Results.Ok(new { 
            Status = "Success",
            Count = usuarios.Count,
            Usuarios = usuarios.Select(u => new {
                u.IdUsuario,
                u.Nome,
                u.Email,
                u.Perfil,
                u.Empresa,
                u.DataCadastro
            }),
            Timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { 
            Status = "Error", 
            Error = ex.Message,
            InnerError = ex.InnerException?.Message,
            StackTrace = ex.StackTrace?.Split('\n').Take(5),
            Timestamp = DateTime.UtcNow 
        });
    }
});

app.Run();

// Expor a classe Program para testes
public partial class Program { }
