using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using challenge_3_net.Models.DTOs;
using challenge_3_net.Services.Interfaces;

namespace challenge_3_net.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de registros de humor do MindTrack
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1")] // Aceitar também v1
    [Produces("application/json")]
    [Authorize(Roles = "PROFISSIONAL,GESTOR")]
    public class HumorController : ControllerBase
    {
        private readonly IHumorService _humorService;
        private readonly ILogger<HumorController> _logger;

        public HumorController(IHumorService humorService, ILogger<HumorController> logger)
        {
            _humorService = humorService;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os registros de humor com paginação
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<HumorResponseDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResultDto<HumorResponseDto>>> ObterTodos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("pageNumber deve ser >= 1 e pageSize deve estar entre 1 e 100");
                }

                var resultado = await _humorService.ObterTodosAsync(pageNumber, pageSize);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter registros de humor");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Busca um registro de humor por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HumorResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HumorResponseDto>> ObterPorId(int id)
        {
            try
            {
                var humor = await _humorService.ObterPorIdAsync(id);
                if (humor == null)
                {
                    return NotFound($"Registro de humor com ID {id} não encontrado");
                }

                return Ok(humor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter registro de humor com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Lista registros de humor de um usuário específico
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(List<HumorResponseDto>), 200)]
        public async Task<ActionResult<List<HumorResponseDto>>> ObterPorUsuario(int usuarioId)
        {
            try
            {
                var humores = await _humorService.ObterPorUsuarioAsync(usuarioId);
                return Ok(humores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter registros de humor do usuário {UsuarioId}", usuarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria um novo registro de humor
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(HumorResponseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<HumorResponseDto>> Criar([FromBody] CriarHumorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var humor = await _humorService.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = humor.IdHumor }, humor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar registro de humor");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualiza um registro de humor existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HumorResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HumorResponseDto>> Atualizar(int id, [FromBody] AtualizarHumorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var humor = await _humorService.AtualizarAsync(id, dto);
                if (humor == null)
                {
                    return NotFound($"Registro de humor com ID {id} não encontrado");
                }

                return Ok(humor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar registro de humor com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Exclui um registro de humor
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var sucesso = await _humorService.ExcluirAsync(id);
                if (!sucesso)
                {
                    return NotFound($"Registro de humor com ID {id} não encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir registro de humor com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}

