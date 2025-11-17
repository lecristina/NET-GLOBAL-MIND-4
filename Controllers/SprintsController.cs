using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using challenge_3_net.Models.DTOs;
using challenge_3_net.Services.Interfaces;

namespace challenge_3_net.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de sprints do MindTrack
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1")] // Aceitar também v1
    [Produces("application/json")]
    [Authorize(Roles = "PROFISSIONAL,GESTOR")]
    public class SprintsController : ControllerBase
    {
        private readonly ISprintService _sprintService;
        private readonly ILogger<SprintsController> _logger;

        public SprintsController(ISprintService sprintService, ILogger<SprintsController> logger)
        {
            _sprintService = sprintService;
            _logger = logger;
        }

        /// <summary>
        /// Lista todas as sprints com paginação
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<SprintResponseDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResultDto<SprintResponseDto>>> ObterTodos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("pageNumber deve ser >= 1 e pageSize deve estar entre 1 e 100");
                }

                var resultado = await _sprintService.ObterTodosAsync(pageNumber, pageSize);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter sprints");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Busca uma sprint por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SprintResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SprintResponseDto>> ObterPorId(int id)
        {
            try
            {
                var sprint = await _sprintService.ObterPorIdAsync(id);
                if (sprint == null)
                {
                    return NotFound($"Sprint com ID {id} não encontrada");
                }

                return Ok(sprint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter sprint com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Lista sprints de um usuário específico
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(List<SprintResponseDto>), 200)]
        public async Task<ActionResult<List<SprintResponseDto>>> ObterPorUsuario(int usuarioId)
        {
            try
            {
                var sprints = await _sprintService.ObterPorUsuarioAsync(usuarioId);
                return Ok(sprints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter sprints do usuário {UsuarioId}", usuarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria uma nova sprint
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SprintResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<SprintResponseDto>> Criar([FromBody] CriarSprintDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var sprint = await _sprintService.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = sprint.IdSprint }, sprint);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar sprint");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualiza uma sprint existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SprintResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<SprintResponseDto>> Atualizar(int id, [FromBody] AtualizarSprintDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var sprint = await _sprintService.AtualizarAsync(id, dto);
                if (sprint == null)
                {
                    return NotFound($"Sprint com ID {id} não encontrada");
                }

                return Ok(sprint);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar sprint com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Exclui uma sprint
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var sucesso = await _sprintService.ExcluirAsync(id);
                if (!sucesso)
                {
                    return NotFound($"Sprint com ID {id} não encontrada");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir sprint com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}

