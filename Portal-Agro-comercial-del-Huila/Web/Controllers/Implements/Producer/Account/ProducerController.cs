using Business.Interfaces.Implements;
using Entity.DTOs.Producer.Producer.Select;
using Entity.DTOs.Producer.Producer.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using Utilities.Helpers.Auth;

namespace Web.Controllers.Implements.Producer.Cuenta
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProducerController : ControllerBase
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly IProducerService _producerService;
        public ProducerController(ILogger<ProducerController> logger, IProducerService producerService)
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


        [HttpGet("sales-number/{codeProducer}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSalesNumberByCode([FromRoute] string codeProducer)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codeProducer))
                    return BadRequest(new { message = "El código del productor es requerido." });
                var salesNumber = await _producerService.SalesNumberByCode(codeProducer);
                return Ok(salesNumber);
            }
            catch (BusinessException be)
            {
                _logger.LogWarning(be, "Error de negocio al obtener número de ventas para productor con código {CodeProducer}", codeProducer);
                return BadRequest(new { message = be.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener número de ventas para productor con código {CodeProducer}", codeProducer);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Se produjo un error inesperado al consultar el número de ventas del productor." });
            }

        }

        [HttpGet("get-code")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCodeProducer()
        {
            int? userId = null;

            try
            {
                userId = HttpContext.TryGetUserId();
                if (userId is null)
                    return Unauthorized(new { message = "No autenticado." });

                var code = await _producerService.GetCodeProducer(userId.Value);

                if (string.IsNullOrWhiteSpace(code))
                    return NotFound(new { message = $"No se encontró código para el usuario {userId}." });

                return Ok(new { code });
            }
            catch (BusinessException be)
            {
                _logger.LogWarning(be, "Error de negocio al obtener código de productor para usuario {UserId}", userId);
                return BadRequest(new { message = be.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener código de productor para usuario {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Se produjo un error inesperado al consultar el código del productor." });
            }
        }

        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile([FromBody] ProducerUpdateDto dto)
        {
            int? userId = null;

            try
            {
                userId = HttpContext.TryGetUserId();
                if (userId is null)
                    return Unauthorized(new { message = "No autenticado." });
                var updated = await _producerService.UpdateProfileAsync(userId.Value, dto);

                if (!updated)
                {
                   
                    _logger.LogWarning("UpdateProfileAsync no aplicó cambios. UserId={UserId}", userId);
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "No se aplicaron cambios en la actualización." });
                }

                return NoContent();
            }
            catch (BusinessException be)
            {
                _logger.LogWarning(be, "Error de negocio al actualizar perfil. Detalle: {Message}", be.Message);
                return BadRequest(new { message = be.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar perfil de productor.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Se produjo un error inesperado al actualizar el perfil." });
            }
        }
    }
}
