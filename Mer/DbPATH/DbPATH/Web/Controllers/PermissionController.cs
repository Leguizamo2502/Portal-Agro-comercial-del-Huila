

using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionBusiness _permissionBusiness;
        private readonly ILogger<PermissionController> _logger;

        /// <summary>
        /// Constructor del controlador de permisos
        /// </summary>
        /// <param name="PermissionBusiness">Capa de negocio de permisos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public PermissionController(PermissionBusiness PermissionBusiness, ILogger<PermissionController> logger)
        {
            _permissionBusiness = PermissionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos del sistema
        /// </summary>
        /// <returns>Lista de permisos</returns>
        /// <response code="200">Retorna la lista de permisos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermissionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                var Permissions = await _permissionBusiness.GetAllPermissionsAsync();
                return Ok(Permissions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un permiso específico por su ID
        /// </summary>
        /// <param name="id">ID del permiso</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el permiso solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            try
            {
                var Permission = await _permissionBusiness.GetPermissionByIdAsync(id);
                return Ok(Permission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo permiso en el sistema
        /// </summary>
        /// <param name="PermissionDto">Datos del permiso a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el permiso creado</response>
        /// <response code="400">Datos del permiso no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(PermissionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDto PermissionDto)
        {
            try
            {
                var createdPermission = await _permissionBusiness.CreatePermissionAsync(PermissionDto);
                return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.Id }, createdPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear permiso");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear permiso");
                return StatusCode(500, new { message = ex.Message });
            }
        }



        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePermissionAsync(int id, [FromBody] PermissionDto PermissionDto)
        {
            try
            {
                if (id != PermissionDto.Id)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del permiso." });
                }

                var updatedPermission = await _permissionBusiness.UpdatePermissionAsync(PermissionDto);
                return Ok(updatedPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar permiso con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar permiso con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar logico
        [HttpPatch("{id}/logical")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalPermissionAsync(int id)
        {
            try
            {
                bool success = await _permissionBusiness.DeletePermissionLogicalAsync(id);
                if (!success)
                    return NotFound(new { message = "permiso no encontrado." });

                return Ok(new { message = "Permiso deshabilitado correctamente." });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al deshabilitar el permiso con ID {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar persistente
        [HttpDelete("{id}/persistence")]
        [ProducesResponseType(204)] // Sin contenido si se elimina correctamente
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePersistencePermissionAsync(int id)
        {
            try
            {
                var deleted = await _permissionBusiness.DeletePermissionPersistenceAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "El permiso no se encontró o ya fue eliminado." });
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar permanentemente el permiso con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permission no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente el permiso con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
