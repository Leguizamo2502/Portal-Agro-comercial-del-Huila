using Business.Interfaces.Implements.Producers.Products;
using Entity.DTOs.Products;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implements.Producer.Products
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpPost("register/product")]
        public async Task<IActionResult> Register([FromForm] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _productService.CreateAsync(dto);
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

    }
}
