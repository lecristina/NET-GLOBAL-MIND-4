using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using challenge_3_net.Models.DTOs;
using challenge_3_net.Services.Interfaces;

namespace challenge_3_net.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de hábitos do MindTrack
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1")] // Aceitar também v1
    [Produces("application/json")]
    [Authorize(Roles = "PROFISSIONAL,GESTOR")]
    public class HabitosController : ControllerBase
    {
        private readonly IHabitoService _habitoService;
        private readonly ILogger<HabitosController> _logger;

        public HabitosController(IHabitoService habitoService, ILogger<HabitosController> logger)
        {
            _habitoService = habitoService;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os hábitos com paginação
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<HabitoResponseDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResultDto<HabitoResponseDto>>> ObterTodos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("pageNumber deve ser >= 1 e pageSize deve estar entre 1 e 100");
                }

                var resultado = await _habitoService.ObterTodosAsync(pageNumber, pageSize);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter hábitos");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Busca um hábito por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HabitoResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HabitoResponseDto>> ObterPorId(int id)
        {
            try
            {
                var habito = await _habitoService.ObterPorIdAsync(id);
                if (habito == null)
                {
                    return NotFound($"Hábito com ID {id} não encontrado");
                }

                return Ok(habito);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter hábito com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Lista hábitos de um usuário específico
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(List<HabitoResponseDto>), 200)]
        public async Task<ActionResult<List<HabitoResponseDto>>> ObterPorUsuario(int usuarioId)
        {
            try
            {
                var habitos = await _habitoService.ObterPorUsuarioAsync(usuarioId);
                return Ok(habitos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter hábitos do usuário {UsuarioId}", usuarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria um novo hábito
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(HabitoResponseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<HabitoResponseDto>> Criar([FromBody] CriarHabitoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var habito = await _habitoService.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = habito.IdHabito }, habito);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar hábito");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Exclui um hábito
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var sucesso = await _habitoService.ExcluirAsync(id);
                if (!sucesso)
                {
                    return NotFound($"Hábito com ID {id} não encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir hábito com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}

