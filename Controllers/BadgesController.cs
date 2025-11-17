using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using nexus.Models.DTOs;
using nexus.Services.Interfaces;

namespace nexus.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de badges do MindTrack
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1")] // Aceitar também v1
    [Produces("application/json")]
    [Authorize(Roles = "PROFISSIONAL,GESTOR")]
    public class BadgesController : ControllerBase
    {
        private readonly IBadgeService _badgeService;
        private readonly ILogger<BadgesController> _logger;

        public BadgesController(IBadgeService badgeService, ILogger<BadgesController> logger)
        {
            _badgeService = badgeService;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os badges com paginação
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<BadgeResponseDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResultDto<BadgeResponseDto>>> ObterTodos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("pageNumber deve ser >= 1 e pageSize deve estar entre 1 e 100");
                }

                var resultado = await _badgeService.ObterTodosAsync(pageNumber, pageSize);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter badges");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Busca um badge por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BadgeResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BadgeResponseDto>> ObterPorId(int id)
        {
            try
            {
                var badge = await _badgeService.ObterPorIdAsync(id);
                if (badge == null)
                {
                    return NotFound($"Badge com ID {id} não encontrado");
                }

                return Ok(badge);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter badge com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Lista badges conquistados por um usuário
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(List<UsuarioBadgeResponseDto>), 200)]
        public async Task<ActionResult<List<UsuarioBadgeResponseDto>>> ObterPorUsuario(int usuarioId)
        {
            try
            {
                var badges = await _badgeService.ObterPorUsuarioAsync(usuarioId);
                return Ok(badges);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter badges do usuário {UsuarioId}", usuarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria um novo badge
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BadgeResponseDto), 201)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "GESTOR")]
        public async Task<ActionResult<BadgeResponseDto>> Criar([FromBody] CriarBadgeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var badge = await _badgeService.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = badge.IdBadge }, badge);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar badge");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualiza um badge existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BadgeResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "GESTOR")]
        public async Task<ActionResult<BadgeResponseDto>> Atualizar(int id, [FromBody] AtualizarBadgeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var badge = await _badgeService.AtualizarAsync(id, dto);
                if (badge == null)
                {
                    return NotFound($"Badge com ID {id} não encontrado");
                }

                return Ok(badge);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar badge com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Concede um badge a um usuário
        /// </summary>
        [HttpPost("usuario/{usuarioId}/badge/{badgeId}")]
        [ProducesResponseType(typeof(UsuarioBadgeResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UsuarioBadgeResponseDto>> ConcederBadge(int usuarioId, int badgeId)
        {
            try
            {
                var usuarioBadge = await _badgeService.ConcederBadgeAsync(usuarioId, badgeId);
                if (usuarioBadge == null)
                {
                    return NotFound("Usuário ou badge não encontrado");
                }

                return CreatedAtAction(nameof(ObterPorUsuario), new { usuarioId }, usuarioBadge);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conceder badge {BadgeId} ao usuário {UsuarioId}", badgeId, usuarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Exclui um badge
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "GESTOR")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var sucesso = await _badgeService.ExcluirAsync(id);
                if (!sucesso)
                {
                    return NotFound($"Badge com ID {id} não encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir badge com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}

