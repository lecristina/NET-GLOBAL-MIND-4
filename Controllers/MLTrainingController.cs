using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using nexus.Services.ML;
using nexus.Models.DTOs;
using nexus.Services.ML.Models;
using System.IO;

namespace nexus.Controllers
{
    /// <summary>
    /// Controller para treinamento de modelos ML.NET
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize] // Requer autenticação JWT
    [Produces("application/json")]
    public class MLTrainingController : ControllerBase
    {
        private readonly MLModelTrainer _modelTrainer;
        private readonly ILogger<MLTrainingController> _logger;

        public MLTrainingController(
            MLModelTrainer modelTrainer,
            ILogger<MLTrainingController> logger)
        {
            _modelTrainer = modelTrainer;
            _logger = logger;
        }

        /// <summary>
        /// Treina o modelo de análise de sentimento com dataset de exemplo
        /// </summary>
        [HttpPost("treinar-sentimento")]
        [ProducesResponseType(200)]
        public IActionResult TreinarModeloSentimento()
        {
            try
            {
                _logger.LogInformation("Iniciando treinamento do modelo de sentimento");

                // Gerar dataset de exemplo
                var dataset = _modelTrainer.GerarDatasetExemplo();
                
                // Salvar dataset
                _modelTrainer.SalvarDatasetEmArquivo(dataset, "sentiment_dataset.csv");

                // Treinar modelo
                var modelo = _modelTrainer.TreinarModelo(dataset, "sentiment_model.zip");

                _logger.LogInformation("Modelo treinado com sucesso");

                return Ok(new
                {
                    Success = true,
                    Message = "Modelo treinado com sucesso",
                    DatasetSize = dataset.Count,
                    ModelPath = "sentiment_model.zip",
                    DatasetPath = "sentiment_dataset.csv"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao treinar modelo");
                return StatusCode(500, new
                {
                    Success = false,
                    Error = "Erro ao treinar modelo",
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Verifica se o modelo treinado existe
        /// </summary>
        [HttpGet("modelo-status")]
        [ProducesResponseType(200)]
        public IActionResult VerificarStatusModelo()
        {
            try
            {
                var modelo = _modelTrainer.CarregarModelo();
                var existe = modelo != null;

                // Verificar se há exemplos customizados
                var exemplosCustomizados = _modelTrainer.CarregarExemplosCustomizados();
                var temExemplosCustomizados = exemplosCustomizados.Any();

                return Ok(new
                {
                    ModeloExiste = existe,
                    TemExemplosCustomizados = temExemplosCustomizados,
                    TotalExemplosCustomizados = exemplosCustomizados.Count,
                    Mensagem = existe 
                        ? "Modelo treinado encontrado e carregado" 
                        : "Modelo treinado não encontrado. Execute o treinamento primeiro."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar status do modelo");
                return StatusCode(500, new
                {
                    Error = "Erro ao verificar status",
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Adiciona exemplos customizados de treinamento
        /// </summary>
        [HttpPost("adicionar-exemplos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AdicionarExemplos([FromBody] AdicionarExemplosTreinamentoDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (request.Exemplos == null || !request.Exemplos.Any())
                {
                    return BadRequest(new { Error = "Lista vazia", Message = "Forneça pelo menos um exemplo de treinamento" });
                }

                // Converter DTOs para SentimentInput
                var exemplos = request.Exemplos.Select(e => new SentimentInput
                {
                    Text = e.Texto,
                    Label = e.Label
                }).ToList();

                // Salvar exemplos customizados
                _modelTrainer.SalvarExemplosCustomizados(exemplos);

                _logger.LogInformation("Adicionados {Count} exemplos customizados de treinamento", exemplos.Count);

                return Ok(new
                {
                    Success = true,
                    Message = $"{exemplos.Count} exemplo(s) adicionado(s) com sucesso",
                    TotalExemplos = _modelTrainer.CarregarExemplosCustomizados().Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar exemplos");
                return StatusCode(500, new
                {
                    Success = false,
                    Error = "Erro ao adicionar exemplos",
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Retreina o modelo com exemplos customizados + dataset padrão
        /// </summary>
        [HttpPost("retreinar-com-exemplos-customizados")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult RetreinarComExemplosCustomizados([FromQuery] bool incluirPadrao = true)
        {
            try
            {
                _logger.LogInformation("Iniciando retreinamento com exemplos customizados (incluir padrão: {IncluirPadrao})", incluirPadrao);

                // Carregar exemplos customizados
                var exemplosCustomizados = _modelTrainer.CarregarExemplosCustomizados();

                if (!exemplosCustomizados.Any() && !incluirPadrao)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Error = "Sem dados",
                        Message = "Não há exemplos customizados salvos e o dataset padrão não foi incluído. Adicione exemplos primeiro ou inclua o dataset padrão."
                    });
                }

                // Combinar datasets
                var datasetCombinado = _modelTrainer.CombinarDatasets(exemplosCustomizados, incluirPadrao);

                if (datasetCombinado.Count < 10)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Error = "Dados insuficientes",
                        Message = $"É necessário pelo menos 10 exemplos para treinar. Atualmente há {datasetCombinado.Count} exemplos."
                    });
                }

                // Salvar dataset combinado
                _modelTrainer.SalvarDatasetEmArquivo(datasetCombinado, "sentiment_dataset.csv");

                // Treinar modelo com métricas
                var (modelo, metricas) = _modelTrainer.TreinarModeloComMetricas(datasetCombinado, "sentiment_model.zip");

                _logger.LogInformation("Modelo retreinado com sucesso. Acurácia: {Accuracy:P2}", metricas.Accuracy);

                return Ok(new TreinamentoResponseDto
                {
                    Success = true,
                    Message = "Modelo retreinado com sucesso usando exemplos customizados",
                    DatasetSize = datasetCombinado.Count,
                    Metrics = new ModelMetricsDto
                    {
                        Accuracy = metricas.Accuracy,
                        Auc = metricas.Auc,
                        F1Score = metricas.F1Score
                    },
                    ModelPath = "sentiment_model.zip",
                    DatasetPath = "sentiment_dataset.csv"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao retreinar modelo");
                return StatusCode(500, new TreinamentoResponseDto
                {
                    Success = false,
                    Message = $"Erro ao retreinar modelo: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Lista exemplos customizados salvos
        /// </summary>
        [HttpGet("exemplos-customizados")]
        [ProducesResponseType(200)]
        public IActionResult ListarExemplosCustomizados()
        {
            try
            {
                var exemplos = _modelTrainer.CarregarExemplosCustomizados();

                var exemplosDto = exemplos.Select(e => new ExemploTreinamentoDto
                {
                    Texto = e.Text,
                    Label = e.Label
                }).ToList();

                return Ok(new
                {
                    Total = exemplos.Count,
                    Positivos = exemplos.Count(e => e.Label),
                    Negativos = exemplos.Count(e => !e.Label),
                    Exemplos = exemplosDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar exemplos customizados");
                return StatusCode(500, new
                {
                    Error = "Erro ao listar exemplos",
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Limpa exemplos customizados salvos
        /// </summary>
        [HttpDelete("exemplos-customizados")]
        [ProducesResponseType(200)]
        public IActionResult LimparExemplosCustomizados()
        {
            try
            {
                var caminhoArquivo = Path.Combine(AppContext.BaseDirectory, "custom_training_data.csv");
                
                if (File.Exists(caminhoArquivo))
                {
                    File.Delete(caminhoArquivo);
                    _logger.LogInformation("Exemplos customizados removidos");
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Exemplos customizados removidos com sucesso"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao limpar exemplos customizados");
                return StatusCode(500, new
                {
                    Success = false,
                    Error = "Erro ao limpar exemplos",
                    Message = ex.Message
                });
            }
        }
    }
}

