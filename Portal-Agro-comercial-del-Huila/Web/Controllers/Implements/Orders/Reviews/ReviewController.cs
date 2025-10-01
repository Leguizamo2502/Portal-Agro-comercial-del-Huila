using Business.Interfaces.Implements.Orders.Reviews;
using Entity.DTOs.Order.Reviews;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using Utilities.Helpers.Auth;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Orders.Reviews
{
    public class ReviewController : BaseController<ReviewCreateDto, ReviewSelectDto, IReviewService>
    {
        public ReviewController(IReviewService service, ILogger<ReviewController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(ReviewCreateDto dto)
        {
            await _service.CreateAsync(dto);
        }

        protected override async Task<bool> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id);
        }

        protected override async Task<bool> DeleteLogicAsync(int id)
        {
            return await _service.DeleteLogicAsync(id);
        }

        protected override async Task<IEnumerable<ReviewSelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task<ReviewSelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, ReviewCreateDto dto)
        {
            return await _service.UpdateAsync(dto);
        }

        [HttpPost("create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public virtual async Task<IActionResult> PostReview([FromBody] ReviewCreateDto dto)
        {
            var userId = HttpContext.GetUserId();
            try
            {
                var result = await _service.CreateReviewAsync(dto, userId);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al agregar");
                return BadRequest(new { IsSucces = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar elemento");
                return StatusCode(500, new { IsSucces = false, message = "Error interno del servidor." });
            }
        }

        [HttpGet("by-product/{productId}")]
        //[ProducesResponseType(typeof(TDto), 200)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<IActionResult> GetByIdProduct(int productId)
        {
            try
            {
                var result = await _service.GetAllByProductId(productId);
                if (result == null)
                    return NotFound(new { message = $"No se encontró el elemento con ID {productId}" });

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida con ID: {Id}", productId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ID {Id}", productId);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

    }
}
