using Business.Interfaces.Implements;
using Entity.DTOs.Producer.Producer.Select;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers.Implements.Producer.Cuenta
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProducerController : ControllerBase
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly IProducerService _producerService;
        public ProducerController(ILogger<ProducerController> logger,IProducerService producerService)
        {
            _logger = logger;
            _producerService = producerService;
        }

        [HttpGet("by-code/{codeProducer}")]
        [ProducesResponseType(typeof(ProducerSelectDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCode([FromRoute] string codeProducer)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codeProducer))
                    return BadRequest(new { message = "El código del productor es requerido." });

                var dto = await _producerService.GetByCodeProducer(codeProducer);

                if (dto is null)
                    return NotFound(new { message = $"No se encontró un productor con el código '{codeProducer}'." });

                return Ok(dto);
            }
            catch (BusinessException be)
            {
                _logger.LogWarning(be, "Error de negocio al obtener productor con código {CodeProducer}", codeProducer);
                return BadRequest(new { message = be.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener productor con código {CodeProducer}", codeProducer);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Se produjo un error inesperado al consultar el productor." });
            }
        }



    }
}
