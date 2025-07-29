using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolFormPermissionController : ControllerBase
    {
        private readonly RolFormPermissionBusiness _rolFormPermissionBusiness;
        private readonly ILogger<RolFormPermissionController> _logger;

        /// <summary>
        /// Constructor del controlador de rolFormPermissions
        /// </summary>
        /// <param name="RolFormPermissionBusiness">Capa de negocio de rolFormPermissions</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public RolFormPermissionController(RolFormPermissionBusiness RolFormPermissionBusiness, ILogger<RolFormPermissionController> logger)
        {
            _rolFormPermissionBusiness = RolFormPermissionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los rolFormPermissions del sistema
        /// </summary>
        /// <returns>Lista de rolFormPermissions</returns>
        /// <response code="200">Retorna la lista de rolFormPermissions</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolFormPermissionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolFormPermissions()
        {
            try
            {
                var RolFormPermissions = await _rolFormPermissionBusiness.GetAllRolFormPermissionAsync();
                return Ok(RolFormPermissions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener rolFormPermissions");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un rolFormPermission específico por su ID
        /// </summary>
        /// <param name="id">ID del rolFormPermission</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el rolFormPermission solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolFormPermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolFormPermissionById(int id)
        {
            try
            {
                var RolFormPermission = await _rolFormPermissionBusiness.GetRolFormPermissionByIdAsync(id);
                return Ok(RolFormPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el rolFormPermission con ID: {RolFormPermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolFormPermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener rolFormPermission con ID: {RolFormPermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo rolFormPermission en el sistema
        /// </summary>
        /// <param name="RolFormPermissionDto">Datos del rolFormPermission a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el rolFormPermission creado</response>
        /// <response code="400">Datos del rolFormPermission no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolFormPermissionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRolFormPermission([FromBody] RolFormPermissionDto RolFormPermissionDto)
        {
            try
            {
                var createdRolFormPermission = await _rolFormPermissionBusiness.CreateRolFormPermissionAsync(RolFormPermissionDto);
                return CreatedAtAction(nameof(GetRolFormPermissionById), new { id = createdRolFormPermission.Id }, createdRolFormPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear rolFormPermission");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear rolFormPermission");
                return StatusCode(500, new { message = ex.Message });
            }
        }



        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RolFormPermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRolFormPermissionAsync(int id, [FromBody] UpdateRolFormPermissionDto RolFormPermissionDto)
        {
            try
            {
                if (id != RolFormPermissionDto.Id)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del rolFormPermission." });
                }

                var updatedRolFormPermission = await _rolFormPermissionBusiness.UpdateRolFormPermissionAsync(RolFormPermissionDto);
                return Ok(updatedRolFormPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar rolFormPermission con ID: {RolFormPermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolFormPermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar rolFormPermission con ID: {RolFormPermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar logico
        [HttpPatch("{id}/logical")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalRolFormPermissionAsync(int id)
        {
            try
            {
                bool success = await _rolFormPermissionBusiness.DeleteRolFormPermissionLogicalAsync(id);
                if (!success)
                    return NotFound(new { message = "rolFormPermission no encontrado." });

                return Ok(new { message = "Permiso deshabilitado correctamente." });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al deshabilitar el rolFormPermission con ID {RolFormPermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar persistente
        [HttpDelete("{id}/persistence")]
        [ProducesResponseType(204)] // Sin contenido si se elimina correctamente
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePersistenceRolFormPermissionAsync(int id)
        {
            try
            {
                var deleted = await _rolFormPermissionBusiness.DeleteRolFormPermissionPersistenceAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "El rolFormPermission no se encontró o ya fue eliminado." });
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar permanentemente el rolFormPermission con ID: {RolFormPermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolFormPermission no encontrado con ID: {RolFormPermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente el rolFormPermission con ID: {RolFormPermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }





    }
}
