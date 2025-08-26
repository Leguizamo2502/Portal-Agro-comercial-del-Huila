using Business.Interfaces.Implements.Producers.Products;
using Entity.DTOs.Products.Select;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implements.Producer.Products
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        private readonly ILogger<ProductImageController> _logger;
        public ProductImageController(IProductImageService productImageService, ILogger<ProductImageController> logger)
        {
            _productImageService = productImageService;
            _logger = logger;
            
        }

        /// <summary>
        /// Subir nuevas imágenes para un prducto (máx. 5 en total)
        /// </summary>
        [HttpPost("{productId}")]
        public async Task<IActionResult> UploadImages(int productId, [FromForm] IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No se han proporcionado archivos.");

            var result = await _productImageService.AddImagesAsync(productId, files);
            return Ok(result);
        }



        /// <summary>
        /// Eliminar una imagen por su ID
        /// </summary>
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            await _productImageService.DeleteImageByIdAsync(imageId);
            return NoContent();
        }

        /// <summary>
        /// Eliminar múltiples imágenes por sus PublicIds
        /// </summary>
        [HttpDelete("multiple")]
        public async Task<IActionResult> DeleteMultipleImages([FromBody] List<string> publicIds)
        {
            if (publicIds == null || !publicIds.Any())
                return BadRequest("Debe proporcionar al menos un PublicId.");

            await _productImageService.DeleteImagesByPublicIdsAsync(publicIds);
            return NoContent();
        }

        [HttpPatch("logical-delete")]
        public async Task<IActionResult> LogicalDelete([FromQuery] string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return BadRequest("El publicId es obligatorio.");

            var result = await _productImageService.DeleteLogicalByPublicIdAsync(publicId);

            if (!result)
                return NotFound($"No se encontró la imagen con publicId '{publicId}'.");

            return NoContent();
        }



        /// <summary>
        /// Obtener todas las imágenes de un producto
        /// </summary>
        [HttpGet("{productId}")]
        public async Task<ActionResult<List<ProductImageSelectDto>>> GetByEstablishment(int productId)
        {
            var images = await _productImageService.GetImagesByProductIdAsync(productId);
            return Ok(images);
        }


    }
}
