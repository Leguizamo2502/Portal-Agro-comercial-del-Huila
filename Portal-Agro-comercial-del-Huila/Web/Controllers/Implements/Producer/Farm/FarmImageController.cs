using Business.Interfaces.Implements.Producers.Farms;
using Entity.DTOs.Products.Select;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implements.Producer.Farm
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FarmImageController : ControllerBase
    {
        private readonly IFarmImageService _farmImageService;
        private readonly ILogger<FarmImageController> _logger;

        public FarmImageController(IFarmImageService farmImageService, ILogger<FarmImageController> logger)
        {
            _logger = logger;
            _farmImageService = farmImageService;
            
        }

        /// <summary>
        /// Subir nuevas imágenes para un prducto (máx. 5 en total)
        /// </summary>
        [HttpPost("{productId}")]
        public async Task<IActionResult> UploadImages(int productId, [FromForm] IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No se han proporcionado archivos.");

            var result = await _farmImageService.AddImagesAsync(productId, files);
            return Ok(result);
        }



        /// <summary>
        /// Eliminar una imagen por su ID
        /// </summary>
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            await _farmImageService.DeleteImageByIdAsync(imageId);
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

            await _farmImageService.DeleteImagesByPublicIdsAsync(publicIds);
            return NoContent();
        }

        [HttpPatch("logical-delete")]
        public async Task<IActionResult> LogicalDelete([FromQuery] string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return BadRequest("El publicId es obligatorio.");

            var result = await _farmImageService.DeleteLogicalByPublicIdAsync(publicId);

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
            var images = await _farmImageService.GetImagesByFarmIdAsync(productId);
            return Ok(images);
        }

    }
}
