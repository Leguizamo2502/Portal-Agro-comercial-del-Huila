using Business.Interfaces.Implements.Producers.Categories;
using Entity.DTOs.Producer.Categories;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.Implements.Producer.Categories
{
    public class CategoryController : BaseController<CategoryRegisterDto, CategorySelectDto, ICategoryService>
    {
        public CategoryController(ICategoryService service, ILogger<CategoryController> logger) : base(service, logger)
        {
        }

        protected override async Task AddAsync(CategoryRegisterDto dto)
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

        protected override async Task<IEnumerable<CategorySelectDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
            }

        protected override async Task<CategorySelectDto?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        protected override async Task<bool> UpdateAsync(int id, CategoryRegisterDto dto)
        {
            return await _service.UpdateAsync(dto);
        }

        /// <summary>
        /// Retorna categorías del nivel solicitado.
        /// Si parentId es null u omitido, retorna categorías raíz.
        /// Si parentId tiene valor, retorna las hijas de esa categoría.
        /// </summary>
        /// <param name="parentId">Id de la categoría padre; null para raíces.</param>
        [HttpGet("node")]
        [ProducesResponseType(typeof(IEnumerable<CategoryNodeDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get([FromQuery] int? parentId)
        {
            try
            {
                var data = await _service.GetNodesAsync(parentId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo categorías (parentId={ParentId})", parentId);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
    }
}
