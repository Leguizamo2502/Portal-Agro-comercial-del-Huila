using Business.Interfaces.Implements.Producers.Products;
using Entity.DTOs.BaseDTO;
using Entity.DTOs.Favorites.Create;
using Entity.DTOs.Products.Create;
using Entity.DTOs.Products.Select;
using Entity.DTOs.Products.Update;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Utilities.Exceptions;
using Utilities.Helpers.Auth;

namespace Web.Controllers.Implements.Producer.Products
{

    [ApiController]
    //[Authorize]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductReadService _productReadService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger, IProductReadService productReadService)
        {
            _productService = productService;
            _productReadService = productReadService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [OutputCache(PolicyName = "ProductsListPolicy")]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                var result = await _productReadService.GetAllAsync();
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
        [OutputCache(PolicyName = "ProductDetailPolicy")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _productReadService.GetByIdAsync(id);
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
                var result = await _productReadService.GetByProducerAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }

        }


        [HttpPost("register/product")]
        public async Task<IActionResult> Register([FromForm] ProductCreateDto dto, [FromServices] IOutputCacheStore cache)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { IsSuccess = false, Errors = ModelState });

            var userId = HttpContext.GetUserId();

            try
            {
                dto.ProducerId = userId;

                var newId = await _productService.CreateProductAsync(dto);
                if (newId <= 0)
                    return BadRequest(new { IsSuccess = false, message = "No se pudo crear el producto." });
                // Evitar cache obsoleto
                await cache.EvictByTagAsync("products", default);

                return Ok(new { IsSuccess = true, message = "Producto creado correctamente." });
            }
            catch (BusinessException bex)
            {
                return BadRequest(new { IsSuccess = false, message = bex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el producto");
                return StatusCode(500, new { IsSuccess = false, message = "Ocurrió un error al registrar el producto." });
            }
        }





        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductUpdateDto dto, [FromServices] IOutputCacheStore cache)
        {
            if (dto is not BaseDto identifiableDto)
                return BadRequest(new { IsSuccess = false, message = "El DTO no implementa IHasId." });

            identifiableDto.Id = id;
            if (id != dto.Id)
                return BadRequest(new { IsSuccess = false, message = "El ID de la URL no coincide con el del cuerpo." });

            var userId = HttpContext.GetUserId();

            try
            {
                var ok = await _productService.UpdateProductAsync(dto, userId);
                if (!ok) return BadRequest(new { IsSuccess = false, message = "No se pudo actualizar el producto." });

                await cache.EvictByTagAsync("products", default);

                return Ok(new { IsSuccess = true, message = "Producto actualizado correctamente." });
            }
            catch (BusinessException bex)
            {
                return BadRequest(new { IsSuccess = false, message = bex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando producto {Id}", id);
                return StatusCode(500, new { IsSuccess = false, message = "Error interno al actualizar el producto." });
            }
        }



        /// <summary>
        /// Eliminar lógicamente un Producto (soft delete).
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, [FromServices] IOutputCacheStore cache)
        {
            await _productService.DeleteLogicAsync(id);
            await cache.EvictByTagAsync("products", default);
            return NoContent();
        }

        [HttpPost("register/favorite")]
        public async Task<IActionResult> RegisterFavorite([FromBody] FavoriteDto dto)
        {
            var userId = HttpContext.GetUserId();
            var created = await _productService.AddFavoriteAsync(userId, dto.ProductId);
            if (created) return StatusCode(StatusCodes.Status201Created);
            return NoContent();
        }

        [HttpDelete("favorite/{productId:int}")]
        public async Task<IActionResult> DeleteFavorite(int productId)
        {
            var userId = HttpContext.GetUserId();

            try
            {
                var removed = await _productService.RemoveFavoriteAsync(userId, productId);

                if (removed)
                    return NoContent(); // 204, borrado exitoso
                else
                    return NotFound(new { IsSuccess = false, message = "El favorito no existe" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { IsSuccess = false, message = "Ocurrió un error al eliminar favorito", error = ex.Message });
            }
        }

        [HttpGet("home")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [OutputCache(PolicyName = "HomeProductsPolicy")]
        public virtual async Task<IActionResult> GetForUser([FromQuery] int? limit)
        {
            try
            {
                var result = await _productReadService.GetAllHomeAsync(limit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }

        }

        [HttpGet("detail/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                int? userId = HttpContext.TryGetUserId();
                var result = await _productReadService.GetDetailProduct(userId, id);

                if (result is null)
                    return NotFound(new { message = $"Producto con ID {id} no encontrado." });

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al obtener producto {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo detalle de producto {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }


        [HttpGet("favorites")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public virtual async Task<IActionResult> GetFavoritesUser()
        {
            var userId = HttpContext.GetUserId();
            try
            {
                var result = await _productReadService.GetFavoritesForUserAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }

        }

        // GET /api/v1/categories/{categoryId}/products
        [HttpGet("categories/{categoryId:int}/products")]
        [ProducesResponseType(typeof(IEnumerable<ProductSelectDto>), 200)]
        [ProducesResponseType(500)]
        [OutputCache(PolicyName = "CategoryProductsPolicy")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var result = await _productReadService.GetByCategoryAsync(categoryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo productos por categoría {CategoryId}", categoryId);
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
                var result = await _productReadService.GetByProducerCodeAsync(producerCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }

        }

        [HttpGet("featured")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [OutputCache(PolicyName = "FeaturedProductsPolicy")]
        public async Task<IActionResult> GetFeatured([FromQuery] int limit = 10)
        {
            try
            {
                var result = await _productReadService.GetFeaturedAsync(limit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo productos destacados");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpPatch("stock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockDto dto, [FromServices] IOutputCacheStore cache)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _productService.UpdateStockAsync(dto);

                if (success)
                {
                    await cache.EvictByTagAsync("products", default);
                    return Ok(new { IsSucces = true, message = "Stock actualizado correctamente." });
                }

                return NotFound(new { IsSucces = false, message = $"No se encontró el producto con id {dto.ProductId}." });
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Error de negocio al actualizar stock para ProductId {ProductId}", dto.ProductId);
                return BadRequest(new { IsSucces = false, message = bex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar stock para ProductId {ProductId}", dto.ProductId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { IsSucces = false, message = "Se produjo un error inesperado al actualizar el stock." });
            }
        }

    }
}
