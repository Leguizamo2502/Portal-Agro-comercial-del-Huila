using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FormController : ControllerBase
    {
        private readonly FormBusiness _formBusiness;
        private readonly ILogger<FormController> _logger;

        /// <summary>
        /// Constructor del controlador de formularios
        /// </summary>
        /// <param name="FormBusiness">Capa de negocio de formularios</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public FormController(FormBusiness FormBusiness, ILogger<FormController> logger)
        {
            _formBusiness = FormBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los formularios del sistema
        /// </summary>
        /// <returns>Lista de formularios</returns>
        /// <response code="200">Retorna la lista de formularios</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllForms()
        {
            try
            {
                var Forms = await _formBusiness.GetAllFormsAsync();
                return Ok(Forms);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener formularios");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un formulario específico por su ID
        /// </summary>
        /// <param name="id">ID del formulario</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el formulario solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormById(int id)
        {
            try
            {
                var Form = await _formBusiness.GetFormByIdAsync(id);
                return Ok(Form);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el formulario con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener formulario con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo formulario en el sistema
        /// </summary>
        /// <param name="FormDto">Datos del formulario a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el formulario creado</response>
        /// <response code="400">Datos del formulario no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(FormDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateForm([FromBody] FormDto FormDto)
        {
            try
            {
                var createdForm = await _formBusiness.CreateFormAsync(FormDto);
                return CreatedAtAction(nameof(GetFormById), new { id = createdForm.Id }, createdForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear formulario");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear formulario");
                return StatusCode(500, new { message = ex.Message });
            }
        }



        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateFormAsync(int id, [FromBody] FormDto FormDto)
        {
            try
            {
                if (id != FormDto.Id)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID del formulario." });
                }

                var updatedForm = await _formBusiness.UpdateFormAsync(FormDto);
                return Ok(updatedForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar formulario con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar formulario con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar logico
        [HttpPatch("{id}/logical")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalFormAsync(int id)
        {
            try
            {
                bool success = await _formBusiness.DeleteFormLogicalAsync(id);
                if (!success)
                    return NotFound(new { message = "formulario no encontrado." });

                return Ok(new { message = "Permiso deshabilitado correctamente." });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al deshabilitar el formulario con ID {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Borrar persistente
        [HttpDelete("{id}/persistence")]
        [ProducesResponseType(204)] // Sin contenido si se elimina correctamente
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePersistenceFormAsync(int id)
        {
            try
            {
                var deleted = await _formBusiness.DeleteFormPersistenceAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "El formulario no se encontró o ya fue eliminado." });
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar permanentemente el formulario con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Form no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente el formulario con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
