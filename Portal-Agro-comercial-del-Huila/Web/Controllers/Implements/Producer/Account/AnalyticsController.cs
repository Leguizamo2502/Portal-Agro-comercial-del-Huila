using Business.Interfaces.Implements.Producers.Analitics;
using Microsoft.AspNetCore.Mvc;
using Utilities.Helpers.Auth;

namespace Web.Controllers.Implements.Producer.Account
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(IAnalyticsService analyticsService,
                                           ILogger<AnalyticsController> logger)
        {
            _analyticsService = analyticsService;
            _logger = logger;
        }

        // GET /analytics/producer/top-products?limit=5
        [HttpGet("top-products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopProducts([FromQuery] int? limit, CancellationToken ct)
        {
            try
            {
                var userId = HttpContext.TryGetUserId();
                if (userId is null) return Forbid();

                var safeLimit = limit.GetValueOrDefault(5);
                if (safeLimit <= 0) safeLimit = 5;
                if (safeLimit > 20) safeLimit = 20;

                var result = await _analyticsService.GetTopProductsForCurrentProducerAsync(userId.Value, safeLimit, ct);
                return Ok(new { items = result, totalProducts = result.Count });
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogWarning(ioe, "Usuario sin productor asociado.");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "No autorizado como productor." });
            }
            catch (ArgumentException ae)
            {
                _logger.LogWarning(ae, "Parámetros inválidos.");
                return BadRequest(new { message = ae.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener top de productos.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor." });
            }
        }
    }
}
