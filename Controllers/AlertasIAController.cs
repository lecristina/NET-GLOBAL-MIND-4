using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using nexus.Models.DTOs;
using nexus.Services.Interfaces;

namespace nexus.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de alertas de IA do MindTrack
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1")] // Aceitar também v1
    [Produces("application/json")]
    [Authorize(Roles = "PROFISSIONAL,GESTOR")]
    public class AlertasIAController : ControllerBase
    {
        private readonly IAlertaIAService _alertaIAService;
        private readonly ILogger<AlertasIAController> _logger;

        public AlertasIAController(IAlertaIAService alertaIAService, ILogger<AlertasIAController> logger)
        {
            _alertaIAService = alertaIAService;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os alertas de IA com paginação
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<AlertaIAResponseDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResultDto<AlertaIAResponseDto>>> ObterTodos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("pageNumber deve ser >= 1 e pageSize deve estar entre 1 e 100");
                }

                var resultado = await _alertaIAService.ObterTodosAsync(pageNumber, pageSize);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter alertas de IA");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Busca um alerta de IA por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AlertaIAResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AlertaIAResponseDto>> ObterPorId(int id)
        {
            try
            {
                var alerta = await _alertaIAService.ObterPorIdAsync(id);
                if (alerta == null)
                {
                    return NotFound($"Alerta de IA com ID {id} não encontrado");
                }

                return Ok(alerta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter alerta de IA com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Lista alertas de IA de um usuário específico
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(List<AlertaIAResponseDto>), 200)]
        public async Task<ActionResult<List<AlertaIAResponseDto>>> ObterPorUsuario(int usuarioId)
        {
            try
            {
                var alertas = await _alertaIAService.ObterPorUsuarioAsync(usuarioId);
                return Ok(alertas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter alertas de IA do usuário {UsuarioId}", usuarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria um novo alerta de IA
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AlertaIAResponseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AlertaIAResponseDto>> Criar([FromBody] CriarAlertaIADto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var alerta = await _alertaIAService.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = alerta.IdAlerta }, alerta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar alerta de IA");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Exclui um alerta de IA
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var sucesso = await _alertaIAService.ExcluirAsync(id);
                if (!sucesso)
                {
                    return NotFound($"Alerta de IA com ID {id} não encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir alerta de IA com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}

