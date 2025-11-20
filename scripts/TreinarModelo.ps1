# Script PowerShell para treinar o modelo ML.NET
# Execute: .\Scripts\TreinarModelo.ps1

Write-Host "=== Treinamento de Modelo ML.NET ===" -ForegroundColor Cyan
Write-Host ""

# Compilar o projeto
Write-Host "1. Compilando projeto..." -ForegroundColor Yellow
dotnet build nexus.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "Erro na compilação!" -ForegroundColor Red
    exit 1
}

Write-Host "   ✅ Compilação concluída" -ForegroundColor Green
Write-Host ""

# Executar script de treinamento
Write-Host "2. Executando treinamento do modelo..." -ForegroundColor Yellow
Write-Host "   (O modelo será salvo em bin/Debug/net9.0/sentiment_model.zip)" -ForegroundColor Gray
Write-Host ""

# Criar um programa temporário para treinar
$scriptContent = @"
using nexus.Services.ML;
using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(builder => {
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});
var logger = loggerFactory.CreateLogger<MLModelTrainer>();

var modelTrainer = new MLModelTrainer(logger);
Console.WriteLine("=== Treinamento de Modelo ML.NET ===\n");

var dataset = modelTrainer.GerarDatasetExemplo();
Console.WriteLine($"Dataset gerado: {dataset.Count} exemplos\n");

modelTrainer.SalvarDatasetEmArquivo(dataset, "sentiment_dataset.csv");
Console.WriteLine("Dataset salvo em sentiment_dataset.csv\n");

var modelo = modelTrainer.TreinarModelo(dataset, "sentiment_model.zip");
Console.WriteLine("\n=== Treinamento concluído! ===");
"@

# Salvar script temporário
$tempScript = "temp_train.cs"
$scriptContent | Out-File -FilePath $tempScript -Encoding UTF8

# Executar via dotnet-script ou compilar e executar
Write-Host "   Executando treinamento..." -ForegroundColor Gray

# Alternativa: usar dotnet run com um endpoint temporário ou criar um projeto console separado
Write-Host "   ⚠️  Para treinar o modelo, execute o código de treinamento manualmente" -ForegroundColor Yellow
Write-Host "   ou use a API endpoint de treinamento (se implementado)" -ForegroundColor Yellow

# Limpar
Remove-Item $tempScript -ErrorAction SilentlyContinue

Write-Host ""
Write-Host "=== Concluído ===" -ForegroundColor Green

