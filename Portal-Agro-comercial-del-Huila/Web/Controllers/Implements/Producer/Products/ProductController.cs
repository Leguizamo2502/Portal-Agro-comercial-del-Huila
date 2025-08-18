using Business.Interfaces.Implements.Producers.Products;
using Entity.DTOs.BaseDTO;
using Entity.DTOs.Products.Create;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;
using Microsoft.AspNetCore.Mvc;
using Utilities.Helpers.Auth;

namespace Web.Controllers.Implements.Producer.Products
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                var result = await _productService.GetAllAsync();
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
                var result = await _productService.GetByIdAsync(id);
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
                var result = await _productService.GetByProducer(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }

        }


        [HttpPost("register/product")]
        public async Task<IActionResult> Register([FromForm] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _productService.CreateProductAsync(dto);
                if (result != null)
                    return Ok(new { IsSuccess = true, message = "Producto creada correctamente" });
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                // Puedes registrar el error para monitoreo
                // _logger.LogError(ex, "Error al registrar la producto");

                return StatusCode(500, new { IsSuccess = false, message = "Ocurrió un error al registrar la producto", error = ex.Message });
            }
        }


        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductSelectDto>> Update(int id, [FromForm] ProductUpdateDto dto)
        {
            if (dto is not BaseDto identifiableDto)
                return BadRequest(new { message = "El DTO no implementa IHasId." });

            identifiableDto.Id = id;
            if (id != dto.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo del formulario.");

            var result = await _productService.UpdateProductAsync(dto);
            return Ok(result);
        }


        /// <summary>
        /// Eliminar lógicamente un Producto (soft delete).
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteLogicAsync(id);
            return NoContent();
        }

    }
}
