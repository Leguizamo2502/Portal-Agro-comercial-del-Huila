using System.Security.Claims;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Services.Producers.Farms;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.BaseDTO;
using Entity.DTOs.Producer.Farm.Create;
using Entity.DTOs.Producer.Farm.Update;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using Utilities.Helpers.Auth;

namespace Web.Controllers.Implements.Producer.Farm
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class FarmController : ControllerBase
    {
        private readonly IFarmService _farmService;
        private readonly ILogger<FarmController> _logger;
        public FarmController(IFarmService farmService, ILogger<FarmController> logger)
        {
            _farmService = farmService;
            _logger = logger;
        }


        [HttpPost("registrar/producer")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Registrarse([FromForm] ProducerWithFarmRegisterDto dto)
        {
            var userId = HttpContext.GetUserId();
            try
            {
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
            var userId = HttpContext.GetUserId();

            try
            {
                dto.ProducerId = userId;
                var result = await _farmService.CreateFarmAsync(dto);
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

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public virtual async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _farmService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }

        }

        [HttpGet("by-producer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public virtual async Task<IActionResult> GetByProducer()
        {
            var userId = HttpContext.GetUserId();
            try
            {
                var result = await _farmService.GetByProducer(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }

        }


        [HttpGet("by-producerCode/{producerCode}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public virtual async Task<IActionResult> GetByProducerCode([FromRoute] string producerCode)
        {
            try
            {
                var result = await _farmService.GetByProducerCodeAsync(producerCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }

        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductSelectDto>> Update(int id, [FromForm] FarmUpdateDto dto)
        {
            if (dto is not BaseDto identifiableDto)
                return BadRequest(new { message = "El DTO no implementa IHasId." });

            identifiableDto.Id = id;
            if (id != dto.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo del formulario.");

            var result = await _farmService.UpdateFarmAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _farmService.DeleteLogicAsync(id);
            return NoContent();
        }


    }

}
