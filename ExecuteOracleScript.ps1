# Script para executar o SQL no Oracle - MindTrack
$connectionString = "Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm555241;Password=230205;Connection Timeout=30;"

# Ler o arquivo SQL
$sqlScript = Get-Content -Path "create-mindtrack-tables.sql" -Raw

# Dividir o script em comandos individuais
$commands = $sqlScript -split ";" | Where-Object { $_.Trim() -ne "" }

try {
    # Criar conexão Oracle
    Add-Type -Path "C:\Program Files\dotnet\shared\Microsoft.NETCore.App\9.0.0\Oracle.ManagedDataAccess.dll" -ErrorAction SilentlyContinue
    
    $connection = New-Object Oracle.ManagedDataAccess.Client.OracleConnection($connectionString)
    $connection.Open()
    
    Write-Host "✅ Conectado ao Oracle com sucesso!"
    
    foreach ($command in $commands) {
        if ($command.Trim() -ne "") {
            try {
                $cmd = $connection.CreateCommand()
                $cmd.CommandText = $command.Trim()
                $cmd.ExecuteNonQuery()
                Write-Host "✅ Comando executado: $($command.Substring(0, [Math]::Min(50, $command.Length)))..."
            }
            catch {
                Write-Host "⚠️  Erro no comando: $($_.Exception.Message)"
            }
        }
    }
    
    $connection.Close()
    Write-Host "✅ Script executado com sucesso!"
}
catch {
    Write-Host "❌ Erro ao conectar: $($_.Exception.Message)"
}
