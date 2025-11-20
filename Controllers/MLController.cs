using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using nexus.Models.DTOs;
using nexus.Services.ML;
using System.Security.Claims;

namespace nexus.Controllers
{
    /// <summary>
    /// Controller para funcionalidades de Machine Learning e IA
    /// Implementa IA Generativa (análise de sentimento) e Visão Computacional (classificação de imagens)
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize] // Requer autenticação JWT
    [Produces("application/json")]
    public class MLController : ControllerBase
    {
        private readonly ILogger<MLController> _logger;
        private readonly ISentimentAnalysisService _sentimentService;
        private readonly IImageClassificationService _imageService;
        private readonly IWellnessAnalysisService _wellnessService;

        public MLController(
            ILogger<MLController> logger,
            ISentimentAnalysisService sentimentService,
            IImageClassificationService imageService,
            IWellnessAnalysisService wellnessService)
        {
            _logger = logger;
            _sentimentService = sentimentService;
            _imageService = imageService;
            _wellnessService = wellnessService;
        }

        /// <summary>
        /// Status das funcionalidades de ML
        /// </summary>
        [HttpGet("status")]
        [ProducesResponseType(200)]
        public IActionResult GetStatus()
        {
            return Ok(new { 
                Status = "ML features ativas",
                Features = new[] {
                    "Análise de Sentimento (IA Generativa)",
                    "Classificação de Imagens (Visão Computacional)",
                    "Análise Completa de Bem-estar",
                    "Geração de Alertas Inteligentes"
                },
                Message = "Funcionalidades de Machine Learning para análise de bem-estar estão disponíveis"
            });
        }

        /// <summary>
        /// Analisa o sentimento de um texto e gera recomendações personalizadas (IA Generativa)
        /// </summary>
        /// <param name="request">DTO com o texto a ser analisado</param>
        /// <returns>Análise de sentimento com recomendações geradas pela IA</returns>
        [HttpPost("sentimento/analisar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AnalisarSentimento([FromBody] AnaliseSentimentoDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resultado = await _sentimentService.AnalisarSentimentoAsync(request.Texto);
                
                _logger.LogInformation("Análise de sentimento realizada com sucesso. Sentimento: {Sentimento}", 
                    resultado.Sentimento);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao analisar sentimento");
                return StatusCode(500, new { 
                    Error = "Erro ao processar análise de sentimento",
                    Message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Classifica uma imagem de ambiente de trabalho e analisa o bem-estar (Visão Computacional)
        /// </summary>
        /// <param name="request">DTO com a imagem em base64 e descrição opcional</param>
        /// <returns>Classificação da imagem com análise de bem-estar do ambiente</returns>
        [HttpPost("imagem/classificar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ClassificarImagem([FromBody] ClassificacaoImagemDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_imageService.ValidarImagem(request.ImagemBase64))
                {
                    return BadRequest(new { 
                        Error = "Imagem inválida",
                        Message = "A imagem fornecida não é válida ou excede o tamanho máximo (10MB)" 
                    });
                }

                var resultado = await _imageService.ClassificarImagemAsync(
                    request.ImagemBase64, 
                    request.Descricao);

                _logger.LogInformation("Classificação de imagem realizada com sucesso. Categoria: {Categoria}", 
                    resultado.Categoria);

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = "Imagem inválida", Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao classificar imagem");
                return StatusCode(500, new { 
                    Error = "Erro ao processar classificação de imagem",
                    Message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Realiza análise completa de bem-estar do usuário autenticado
        /// Integra análise de sentimento, produtividade e gera alertas inteligentes
        /// </summary>
        /// <returns>Análise completa de bem-estar com recomendações e alertas</returns>
        [HttpGet("bem-estar/analise-completa")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AnalisarBemEstarCompleto()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { Error = "Token inválido", Message = "Não foi possível identificar o usuário" });
                }

                var resultado = await _wellnessService.AnalisarBemEstarCompletoAsync(userId);

                _logger.LogInformation("Análise completa de bem-estar realizada para usuário {UserId}. Score: {Score}", 
                    userId, resultado.ScoreBemEstar);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao analisar bem-estar completo");
                return StatusCode(500, new { 
                    Error = "Erro ao processar análise de bem-estar",
                    Message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Gera alertas inteligentes baseados nos dados do usuário autenticado
        /// </summary>
        /// <returns>Lista de alertas gerados pela IA</returns>
        [HttpGet("alertas/gerar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GerarAlertasInteligentes()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { Error = "Token inválido", Message = "Não foi possível identificar o usuário" });
                }

                var alertas = await _wellnessService.GerarAlertasInteligentesAsync(userId);

                _logger.LogInformation("Alertas inteligentes gerados para usuário {UserId}. Total: {Total}", 
                    userId, alertas.Count);

                return Ok(new { 
                    UsuarioId = userId,
                    TotalAlertas = alertas.Count,
                    Alertas = alertas,
                    DataGeracao = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar alertas inteligentes");
                return StatusCode(500, new { 
                    Error = "Erro ao gerar alertas",
                    Message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Analisa o sentimento de múltiplos textos (útil para análise de histórico)
        /// </summary>
        /// <param name="textos">Lista de textos a serem analisados</param>
        /// <returns>Análise agregada de sentimento</returns>
        [HttpPost("sentimento/analisar-multiplos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AnalisarSentimentosMultiplos([FromBody] List<string> textos)
        {
            try
            {
                if (textos == null || !textos.Any())
                {
                    return BadRequest(new { Error = "Lista vazia", Message = "Forneça pelo menos um texto para análise" });
                }

                var resultado = await _sentimentService.AnalisarSentimentosAsync(textos);

                _logger.LogInformation("Análise de múltiplos sentimentos realizada. Total textos: {Total}, Sentimento: {Sentimento}", 
                    textos.Count, resultado.Sentimento);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao analisar múltiplos sentimentos");
                return StatusCode(500, new { 
                    Error = "Erro ao processar análise de sentimentos",
                    Message = ex.Message 
                });
            }
        }
    }
}
