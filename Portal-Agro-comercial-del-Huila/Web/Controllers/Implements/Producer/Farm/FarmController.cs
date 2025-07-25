using System.Security.Claims;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Services.Producers.Farms;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Producer.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers.Implements.Producer.Farm
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly IFarmService _farmService;
        private readonly ILogger<FarmController> _logger;
        private readonly IFarmImageService _farmImageService;
        public FarmController(IFarmService farmService, ILogger<FarmController> logger, IFarmImageService farmImageService)
        {
            _farmService = farmService;
            _logger = logger;
            _farmImageService = farmImageService;
        }


        [HttpPost]
        [Authorize]
        [Route("registrar/producer")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Registrarse(ProducerWithFarmRegisterDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("El token no contiene un Claim 'sub' (NameIdentifier) válido o no es un ID.");

                var userCreated = await _farmService.RegisterWithProducer(dto, userId);

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { isSuccess = false, message = ex.Message });
            }
        }


        [HttpPost("register/farm")]
    
        public async Task<IActionResult> Register([FromForm] FarmRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _farmService.CreateAsync(dto);
                if(result !=null)
                    return Ok(new { IsSuccess = true, message = "Finca creada correctamente"});
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                // Puedes registrar el error para monitoreo
                // _logger.LogError(ex, "Error al registrar la finca");

                return StatusCode(500, new { IsSuccess = false, message = "Ocurrió un error al registrar la finca", error = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                var result = await _farmService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }

        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> Delete(int imageId)
        {
            try
            {
                await _farmImageService.DeleteImageAsync(imageId);
                return NoContent(); // 204
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message }); // 400
            }
            catch (Exception ex)
            {
                // Puedes loguear el error si tienes un logger aquí
                return StatusCode(500, new { message = "Error interno al eliminar la imagen." });
            }
        }

    }

}
