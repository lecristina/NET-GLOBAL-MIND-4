using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace challenge_3_net.Controllers
{
    /// <summary>
    /// Controller para funcionalidades de Machine Learning (TODO: Implementar para MindTrack)
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize] // Requer autenticação JWT
    [Produces("application/json")]
    public class MLController : ControllerBase
    {
        private readonly ILogger<MLController> _logger;

        public MLController(ILogger<MLController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Endpoint placeholder para funcionalidades de ML (a ser implementado)
        /// </summary>
        [HttpGet("status")]
        [ProducesResponseType(200)]
        public IActionResult GetStatus()
        {
            return Ok(new { 
                Status = "ML features em desenvolvimento",
                Message = "Funcionalidades de Machine Learning para análise de bem-estar serão implementadas em breve"
            });
        }
    }
}
