
using Utilities.Exceptions;
using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly UserBusiness _userBusiness;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Constructor del controlador de usuarios
        /// </summary>
        /// <param name="UserBusiness">Capa de negocio de usuarios</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public UserController(UserBusiness UserBusiness, ILogger<UserController> logger)
        {
            _userBusiness = UserBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los usuarios del sistema
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        /// <response code="200">Retorna la lista de usuarios</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var Users = await _userBusiness.GetAllUsersAsync();
                return Ok(Users);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un usuario específico por su ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el usuario solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var User = await _userBusiness.GetUserByIdAsync(id);
                return Ok(User);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el usuario con ID: {UserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {UserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema
        /// </summary>
        /// <param name="UserDto">Datos del usuario a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el usuario creado</response>
        /// <response code="400">Datos del usuario no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto UserDto)
        {
            try
            {
                var createdUser = await _userBusiness.CreateUserAsync(UserDto);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear usuario");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return StatusCode(500, new { message = ex.Message });
            }
        }



        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserDto UserDto)
        {
            try
            {
                if (id != UserDto.Id)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del usuario." });
                }

                var updatedUser = await _userBusiness.UpdateUserAsync(UserDto);
                return Ok(updatedUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar usuario con ID: {UserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {UserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar logico
        [HttpPatch("{id}/logical")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalUserAsync(int id)
        {
            try
            {
                bool success = await _userBusiness.DeleteUserLogicalAsync(id);
                if (!success)
                    return NotFound(new { message = "usuario no encontrado." });

                return Ok(new { message = "Permiso deshabilitado correctamente." });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al deshabilitar el usuario con ID {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar persistente
        [HttpDelete("{id}/persistence")]
        [ProducesResponseType(204)] // Sin contenido si se elimina correctamente
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePersistenceUserAsync(int id)
        {
            try
            {
                var deleted = await _userBusiness.DeleteUserPersistenceAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "El usuario no se encontró o ya fue eliminado." });
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar permanentemente el usuario con ID: {UserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "User no encontrado con ID: {UserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente el usuario con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }



    }
}
