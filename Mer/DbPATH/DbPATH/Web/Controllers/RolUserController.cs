using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolUserController : ControllerBase
    {
        private readonly RolUserBusiness _rolUserBusiness;
        private readonly ILogger<RolUserController> _logger;

        /// <summary>
        /// Constructor del controlador de rolUsers
        /// </summary>
        /// <param name="RolUserBusiness">Capa de negocio de rolUsers</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public RolUserController(RolUserBusiness RolUserBusiness, ILogger<RolUserController> logger)
        {
            _rolUserBusiness = RolUserBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los rolUsers del sistema
        /// </summary>
        /// <returns>Lista de rolUsers</returns>
        /// <response code="200">Retorna la lista de rolUsers</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolUserDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolUsers()
        {
            try
            {
                var RolUsers = await _rolUserBusiness.GetAllRolUserAsync();
                return Ok(RolUsers);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener rolUsers");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un rolUser específico por su ID
        /// </summary>
        /// <param name="id">ID del rolUser</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el rolUser solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolUserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolUserById(int id)
        {
            try
            {
                var RolUser = await _rolUserBusiness.GetRolUserByIdAsync(id);
                return Ok(RolUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el rolUser con ID: {RolUserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolUserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener rolUser con ID: {RolUserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo rolUser en el sistema
        /// </summary>
        /// <param name="RolUserDto">Datos del rolUser a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el rolUser creado</response>
        /// <response code="400">Datos del rolUser no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolUserDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRolUser([FromBody] RolUserDto RolUserDto)
        {
            try
            {
                var createdRolUser = await _rolUserBusiness.CreateRolUserAsync(RolUserDto);
                return CreatedAtAction(nameof(GetRolUserById), new { id = createdRolUser.Id }, createdRolUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear rolUser");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear rolUser");
                return StatusCode(500, new { message = ex.Message });
            }
        }



        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RolUserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRolUserAsync(int id, [FromBody] UpdateRolUserDto RolUserDto)
        {
            try
            {
                if (id != RolUserDto.Id)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del rolUser." });
                }

                var updatedRolUser = await _rolUserBusiness.UpdateRolUserAsync(RolUserDto);
                return Ok(updatedRolUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar rolUser con ID: {RolUserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolUserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar rolUser con ID: {RolUserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar logico
        [HttpPatch("{id}/logical")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalRolUserAsync(int id)
        {
            try
            {
                bool success = await _rolUserBusiness.DeleteFormLogicalAsync(id);
                if (!success)
                    return NotFound(new { message = "rolUser no encontrado." });

                return Ok(new { message = "Permiso deshabilitado correctamente." });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al deshabilitar el rolUser con ID {RolUserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar persistente
        [HttpDelete("{id}/persistence")]
        [ProducesResponseType(204)] // Sin contenido si se elimina correctamente
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePersistenceRolUserAsync(int id)
        {
            try
            {
                var deleted = await _rolUserBusiness.DeleteFormPersistentAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "El rolUser no se encontró o ya fue eliminado." });
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar permanentemente el rolUser con ID: {RolUserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolUser no encontrado con ID: {RolUserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente el rolUser con ID: {RolUserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
