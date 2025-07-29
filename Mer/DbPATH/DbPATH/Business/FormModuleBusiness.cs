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
    public class FormModuleBusiness
    {
        private readonly FormModuleData _formModuleData;
        private readonly ILogger<FormModuleBusiness> _logger;

        public FormModuleBusiness(FormModuleData formModuleData, ILogger<FormModuleBusiness> logger)
        {
            _formModuleData = formModuleData;
            _logger = logger;
        }

        // Método para obtener todos los FormModule como DTOs
        public async Task<IEnumerable<FormModuleDto>> GetAllFormModuleAsync()
        {
            try
            {
                var formModules = await _formModuleData.GetAllAsync();
                //var formModulesDTO = MapToDTOList(formModules);
                return formModules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los FormModule");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de FormModule", ex);
            }

        }


        // Método para obtener un FormModule por ID como DTO
        public async Task<FormModuleDto> GetFormModuleByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intento obtener un formModule con un ID inválido: {FormModuleId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del formModule debe ser mayor a 0");
            }

            try
            {
                var formModule = await _formModuleData.GetByIdDtoAsync(id);
                if (formModule == null)
                {
                    _logger.LogInformation("No se encontro ninungo formModule con el id {FormModuleId}", id);
                    throw new EntityNotFoundException("FormModule", id);
                }
                return formModule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formModule con ID {FormModuleId}", id);
                throw new ExternalServiceException("Base de datos", "Error al recuperar el formModule", ex);
            }
        }

        //Metodo para crear un FormModule desde un DTO
        public async Task<FormModuleDto> CreateFormModuleAsync(FormModuleDto FormModuleDto)
        {
            try
            {
                ValidateFormModule(FormModuleDto);
                var formModule = MapToEntity(FormModuleDto);

                var formModuleCreado = await _formModuleData.CreateAsync(formModule);
                return MapToDTO(formModuleCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el formModule: {FormModuleNombre}", FormModuleDto?.FormName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el formModule", ex);
            }

        }

        // Metodo para actualizar
        public async Task<UpdateFormModuleDto> UpdateFormModuleAsync(UpdateFormModuleDto formModuleDto)
        {
            try
            {
                if (formModuleDto == null || formModuleDto.Id <= 0)
                {
                    throw new ValidationException("id", "El ID del formModule debe ser mayor que cero y no nulo");
                }

                var existingFormModule = await _formModuleData.GetByIdAsync(formModuleDto.Id);
                if (existingFormModule == null)
                {
                    throw new EntityNotFoundException("FormModule", formModuleDto.Id);
                }

                existingFormModule.FormId = formModuleDto.FormId;
                existingFormModule.ModuleId = formModuleDto.ModuleId;

                bool success = await _formModuleData.UpdateAsync(existingFormModule);

                if (!success)
                {
                    throw new Exception("No se pudo actualizar el formModule.");
                }

                return new UpdateFormModuleDto
                {
                    Id = formModuleDto.Id,
                    FormId = formModuleDto.FormId,
                    ModuleId = formModuleDto.ModuleId,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el formModule con ID {formModuleDto?.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el formModule con ID {formModuleDto?.Id}", ex);
            }
        }

        //Metodo para borrar logicamente
        public async Task<bool> DeleteFormModuleLogicalAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del formModule debe ser mayor que cero");
                }

                var existingFormModule = await _formModuleData.GetByIdAsync(id);
                if (existingFormModule == null)
                {
                    throw new EntityNotFoundException("FormModule", id);
                }

                return await _formModuleData.DeleteLogicAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al realizar la eliminación lógica del formModule con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al realizar la eliminación lógica del formModule con ID {id}", ex);
            }
        }

        //Metodo para borrar persistente
        public async Task<bool> DeleteFormModulePersistenceAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del formModule debe ser mayor que cero");
                }

                var existingFormModule = await _formModuleData.GetByIdAsync(id);
                if (existingFormModule == null)
                {
                    throw new EntityNotFoundException("FormModule", id);
                }

                return await _formModuleData.DeletePersistenceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar permanentemente el formModule con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el formModule con ID {id}", ex);
            }
        }

        // Método para validar un FormModuleDTO
        public void ValidateFormModule(FormModuleDto FormModuleDto)
        {
            if (FormModuleDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("FormModule", "El formModule no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(FormModuleDto.FormName) && string.IsNullOrWhiteSpace(FormModuleDto.ModuleName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un formModule con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del formModule es obligatorio");
            }

        }




        // Método para mapear de FormModule a FormModuleDTO
        private FormModuleDto MapToDTO(FormModule formModule)
        {
            return new FormModuleDto
            {
                Id = formModule.Id,
                FormId = formModule.FormId,
                ModuleId = formModule.ModuleId,
                //FormName = formModule.Form.Name,
                //ModuleName = formModule.Module.Name,
                IsDeleted = formModule.IsDeleted,
            };
        }

        // Método para mapear de FormModuleDTO a FormModule
        private FormModule MapToEntity(FormModuleDto formModuleDto)
        {
            return new FormModule
            {
                Id = formModuleDto.Id,
                ModuleId = formModuleDto.ModuleId,
                FormId = formModuleDto.FormId,
                IsDeleted = formModuleDto.IsDeleted,


            };
        }


        // Método para mapear una lista de FormModule a una lista de FormModuleDTO
        private IEnumerable<FormModuleDto> MapToDTOList(IEnumerable<FormModule> formModules)
        {
            var formModulesDTO = new List<FormModuleDto>();
            foreach (var formModule in formModules)
            {
                formModulesDTO.Add(MapToDTO(formModule));
            }
            return formModulesDTO;
        }


    }
}
