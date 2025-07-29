using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class ModuleBusiness
    {
        private readonly ModuleData _moduleData;
        private readonly ILogger<ModuleBusiness> _logger;

        public ModuleBusiness(ModuleData moduleData, ILogger<ModuleBusiness> logger)
        {
            _moduleData = moduleData;
            _logger = logger;
        }

        // Método para obtener todos los módulos como DTOs

        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        {
            try
            {
                var modules = await _moduleData.GetAllAsync();
                //var modulesDTO = MapToDTOList(modules);
                //return modulesDTO;
                return MapToDTOList(modules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los módulos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de módulos", ex);
            }
        }

        // Método para obtener un módulo por ID como DTO
        public async Task<ModuleDto> GetModuleByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intento obtener un módulo con un ID inválido: {ModuleId}", id);
                throw new Utilities.Exceptions.ValidationException("ID", "El ID del módulo debe ser mayor a cero");
            }
            try
            {
                var module = await _moduleData.GetByIdAsync(id);
                if (module == null)
                {
                    _logger.LogInformation("Se intento obtener un módulo inexistente con ID: {ModuleId}", id);
                    throw new EntityNotFoundException("Módulo", id);
                }
                return MapToDTO(module);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el módulo con ID: {ModuleId}", id);
                throw new ExternalServiceException("Base de datos", "Error al recuperar el módulo solicitado", ex);
            }
        }

        // Método para crear un módulo desde un DTO
        public async Task<ModuleDto> CreateModuleAsync(ModuleDto moduleDto)
        {
           
            try
            {
                ValidateModule(moduleDto);
                var module = MapToEntity(moduleDto);
                var createdModule = await _moduleData.CreateAsync(module);
                return MapToDTO(createdModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el módulo: {ModuleDto}", moduleDto);
                throw new ExternalServiceException("Base de datos", "Error al crear el módulo", ex);
            }
        }

        // Metodo para actualizar
        public async Task<ModuleDto> UpdateModuleAsync(ModuleDto moduleDto)
        {
            try
            {
                if (moduleDto == null || moduleDto.Id <= 0)
                {
                    throw new ValidationException("id", "El ID del module debe ser mayor que cero y no nulo");
                }

                var existingModule = await _moduleData.GetByIdAsync(moduleDto.Id);
                if (existingModule == null)
                {
                    throw new EntityNotFoundException("Module", moduleDto.Id);
                }

                var updatedModule = MapToEntity(moduleDto);
                bool success = await _moduleData.UpdateAsync(updatedModule);

                if (!success)
                {
                    throw new Exception("No se pudo actualizar el module.");
                }

                return MapToDTO(updatedModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el module con ID {moduleDto?.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el module con ID {moduleDto?.Id}", ex);
            }
        }

        //Metodo para borrar logicamente
        public async Task<bool> DeleteModuleLogicalAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del module debe ser mayor que cero");
                }

                var existingModule = await _moduleData.GetByIdAsync(id);
                if (existingModule == null)
                {
                    throw new EntityNotFoundException("Module", id);
                }

                return await _moduleData.DeleteLogicAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al realizar la eliminación lógica del module con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al realizar la eliminación lógica del module con ID {id}", ex);
            }
        }

        //Metodo para borrar persistente
        public async Task<bool> DeleteModulePersistenceAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del module debe ser mayor que cero");
                }

                var existingModule = await _moduleData.GetByIdAsync(id);
                if (existingModule == null)
                {
                    throw new EntityNotFoundException("Module", id);
                }

                return await _moduleData.DeletePersistenceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar permanentemente el module con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el module con ID {id}", ex);
            }
        }

        // Metodo para validar el DTO
        public void ValidateModule(ModuleDto ModuleDto) 
        {
            if (ModuleDto == null) { 
                throw new Utilities.Exceptions.ValidationException("ModuleDto", "El módulo no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(ModuleDto.Name)) 
            {
                _logger.LogWarning("Se intento crear un módulo con nombre nulo o vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El nombre del módulo no puede ser nulo o vacío");
            }
        }



        //Metodo para mapear de Module a ModuleDto
        private ModuleDto MapToDTO(Module module)
        {
            return new ModuleDto
            {
                Id = module.Id,
                Name = module.Name,
                Description = module.Description,
                IsDeleted = module.IsDeleted,
            };
        }

        //Metodo para mapear de ModuleDto a Module
        private Module MapToEntity(ModuleDto moduleDto)
        {
            return new Module
            {
                Id = moduleDto.Id,
                Name = moduleDto.Name,
                Description = moduleDto.Description,
                IsDeleted = moduleDto.IsDeleted,
            };
        }

        //Metodo para mapar una lista de Module a una lista de ModuleDto
        private IEnumerable<ModuleDto> MapToDTOList(IEnumerable<Module> modules)
        {
            var modulesDTO = new List<ModuleDto>();
            foreach (var module in modules)
            {
                modulesDTO.Add(MapToDTO(module));
            }
            return modulesDTO;
        }


    }
}
