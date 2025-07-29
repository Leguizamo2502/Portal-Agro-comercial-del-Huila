using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class FormBusiness
    {
        private readonly FormData _formData;
        private readonly ILogger<FormBusiness> _logger;

        public FormBusiness(FormData formData, ILogger<FormBusiness> logger)
        {
            _formData = formData;
            _logger = logger;
        }

        // Método para obtener todos los formularios como DTOs
        public async Task<IEnumerable<FormDto>> GetAllFormsAsync()
        {
            try
            {
                var forms = await _formData.GetAllAsync();
                var formsDTO = MapToDTOList(forms);
                return formsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
            }
        }


        // Método para obtener un formulario por ID como DTO
        public async Task<FormDto> GetFormByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intento obtener un formulario con un ID inválido: {FormId}", id);
                throw new Utilities.Exceptions.ValidationException("Formulario", "ID form debe ser mayor a 0");
            }
            try
            {
                var form = await _formData.GetByIdAsync(id);
                if (form == null)
                {
                    _logger.LogWarning("No se encontró el formulario con ID {FormId}", id);
                    throw new EntityNotFoundException("Formulario", id);
                }
                return MapToDTO(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID {FormId}", id);
                throw new ExternalServiceException("Base de datos", "Error al recuperar el formulario", ex);
            }
        }

        //Metodo para crear un formulario
        public async Task<FormDto> CreateFormAsync(FormDto formDto)
        {
            ValidateForm(formDto);
            try
            {
                var form = MapToEntity(formDto);
                var createdForm = await _formData.CreateAsync(form);
                return MapToDTO(createdForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el formulario");
                throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
            }
        }

        // Metodo para actualizar
        public async Task<FormDto> UpdateFormAsync(FormDto formDto)
        {
            try
            {
                if (formDto == null || formDto.Id <= 0)
                {
                    throw new ValidationException("id", "El ID del form debe ser mayor que cero y no nulo");
                }

                var existingForm = await _formData.GetByIdAsync(formDto.Id);
                if (existingForm == null)
                {
                    throw new EntityNotFoundException("Form", formDto.Id);
                }

                var updatedForm = MapToEntity(formDto);
                bool success = await _formData.UpdateAsync(updatedForm);

                if (!success)
                {
                    throw new Exception("No se pudo actualizar el form.");
                }

                return MapToDTO(updatedForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el form con ID {formDto?.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el form con ID {formDto?.Id}", ex);
            }
        }

        //Metodo para borrar logicamente
        public async Task<bool> DeleteFormLogicalAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del form debe ser mayor que cero");
                }

                var existingForm = await _formData.GetByIdAsync(id);
                if (existingForm == null)
                {
                    throw new EntityNotFoundException("Form", id);
                }

                return await _formData.DeleteLogicAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al realizar la eliminación lógica del form con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al realizar la eliminación lógica del form con ID {id}", ex);
            }
        }

        //Metodo para borrar persistente
        public async Task<bool> DeleteFormPersistenceAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del form debe ser mayor que cero");
                }

                var existingForm = await _formData.GetByIdAsync(id);
                if (existingForm == null)
                {
                    throw new EntityNotFoundException("Form", id);
                }

                return await _formData.DeletePersistenceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar permanentemente el form con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el form con ID {id}", ex);
            }
        }


        //Metodo para validar el DTP
        public void ValidateForm(FormDto FormDto)
        {
            if (FormDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("Formulario", "El formulario no puede ser nulo");
            }
            if(string.IsNullOrWhiteSpace(FormDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un formulario con nombre vacío");
                throw new Utilities.Exceptions.ValidationException("Nombre", "El nombre del formulario no puede ser nulo o vacío");
            }
        }

        //Metodo para mapear de Form a FormDto
        private FormDto MapToDTO(Form form)
        {
            return new FormDto
            {
                Id = form.Id,
                Name = form.Name,
                Description = form.Description,
                IsDeleted = form.IsDeleted,
            };
        }

        //Metodo para mapear de FormDto a Form

        private Form MapToEntity(FormDto formDto)
        {
            return new Form
            {
                Id = formDto.Id,
                Name = formDto.Name,
                Description = formDto.Description,
                IsDeleted = formDto.IsDeleted,
            };
        }

        //Metodo par mapear una lista de Form a FormDto
        private IEnumerable<FormDto> MapToDTOList(IEnumerable<Form> forms)
        {
            var formsDTO = new List<FormDto>();
            foreach (var form in forms)
            {
                formsDTO.Add(MapToDTO(form));
            }
            return formsDTO;
        }




    }
}
