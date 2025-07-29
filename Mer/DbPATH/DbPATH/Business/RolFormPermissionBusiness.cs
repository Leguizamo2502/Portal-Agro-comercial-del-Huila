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
    public class RolFormPermissionBusiness
    {
        private readonly RolFormPermissionData _rolFormPermissionData;
        private readonly ILogger<RolFormPermissionBusiness> _logger;

        public RolFormPermissionBusiness(RolFormPermissionData rolFormPermissionData, ILogger<RolFormPermissionBusiness> logger)
        {
            _rolFormPermissionData = rolFormPermissionData;
            _logger = logger;
        }

        // Método para obtener todos los RolFormPermission como DTOs
        public async Task<IEnumerable<RolFormPermissionDto>> GetAllRolFormPermissionAsync()
        {
            try
            {
                var rolFormPermission = await _rolFormPermissionData.GetAllAsync();
                //var rolFormPermissionDTO = MapToDTOList(rolFormPermission);
                return rolFormPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los rolFormPermission");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de rolFormPermission", ex);
            }

        }

        // Método para obtener un RolFormPermission por ID como DTO
        public async Task<RolFormPermissionDto> GetRolFormPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intento obtener un rol con un ID inválido: {RolFormPermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rolFormPermission debe ser mayor a 0");
            }

            try
            {
                var rolFormPermission = await _rolFormPermissionData.GetByIdDtoAsync(id);
                if (rolFormPermission == null)
                {
                    _logger.LogInformation("No se encontro ninungo rolFormPermission con el id {RolFormPermissionId}", id);
                    throw new EntityNotFoundException("RolFormPermission", id);
                }
                return rolFormPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rolFormPermission con ID {RolFormPermissionId}", id);
                throw new ExternalServiceException("Base de datos", "Error al recuperar el rolFormPermission", ex);
            }
        }

        //Metodo para crear un RolFormPermission desde un DTO
        public async Task<RolFormPermissionDto> CreateRolFormPermissionAsync(RolFormPermissionDto RolFormPermissionDto)
        {
            try
            {
                ValidateRolFormPermission(RolFormPermissionDto);
                var rolFormPermission = MapToEntity(RolFormPermissionDto);

                var rolFormPermissionCreado = await _rolFormPermissionData.CreateAsync(rolFormPermission);
                return MapToDTO(rolFormPermissionCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el rolFormPermission: {RolFormPermissionNombre}", RolFormPermissionDto?.RolName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rolFormPermission", ex);
            }

        }

        // Metodo para actualizar
        public async Task<UpdateRolFormPermissionDto> UpdateRolFormPermissionAsync(UpdateRolFormPermissionDto rolFormPermissionDto)
        {
            try
            {
                if (rolFormPermissionDto == null || rolFormPermissionDto.Id <= 0)
                {
                    throw new ValidationException("id", "El ID del rolFormPermissionDto debe ser mayor que cero y no nulo");
                }

                var existingRolFormPermissionDto = await _rolFormPermissionData.GetByIdAsync(rolFormPermissionDto.Id);
                if (existingRolFormPermissionDto == null)
                {
                    throw new EntityNotFoundException("RolFormPermissionDto", rolFormPermissionDto.Id);
                }
                existingRolFormPermissionDto.RolId = rolFormPermissionDto.RolId;
                existingRolFormPermissionDto.FormId = rolFormPermissionDto.FormId;
                existingRolFormPermissionDto.PermissionId = rolFormPermissionDto.PermissionId;


                //var updatedRolFormPermissionDto = MapToEntity(rolFormPermissionDto);
                bool success = await _rolFormPermissionData.UpdateAsync(existingRolFormPermissionDto);

                if (!success)
                {
                    throw new Exception("No se pudo actualizar el rolFormPermissionDto.");
                }

                return new UpdateRolFormPermissionDto
                {
                    Id = existingRolFormPermissionDto.Id,
                    FormId = existingRolFormPermissionDto.FormId,
                    RolId = existingRolFormPermissionDto.RolId,
                    PermissionId = existingRolFormPermissionDto.PermissionId,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el rolFormPermissionDto con ID {rolFormPermissionDto?.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el rolFormPermissionDto con ID {rolFormPermissionDto?.Id}", ex);
            }
        }

        //Metodo para borrar logicamente
        public async Task<bool> DeleteRolFormPermissionLogicalAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del rolFormPermissionDto debe ser mayor que cero");
                }

                var existingRolFormPermission = await _rolFormPermissionData.GetByIdAsync(id);
                if (existingRolFormPermission == null)
                {
                    throw new EntityNotFoundException("RolFormPermission", id);
                }

                return await _rolFormPermissionData.DeleteLogicAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al realizar la eliminación lógica del rolFormPermission con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al realizar la eliminación lógica del rolFormPermission con ID {id}", ex);
            }
        }

        //Metodo para borrar persistente
        public async Task<bool> DeleteRolFormPermissionPersistenceAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del rolFormPermission debe ser mayor que cero");
                }

                var existingRolFormPermission = await _rolFormPermissionData.GetByIdAsync(id);
                if (existingRolFormPermission == null)
                {
                    throw new EntityNotFoundException("RolFormPermission", id);
                }

                return await _rolFormPermissionData.DeletePersistenceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar permanentemente el rolFormPermission con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el rolFormPermission con ID {id}", ex);
            }
        }

        //Metodo para validar el DTO
        public void ValidateRolFormPermission(RolFormPermissionDto RolFormPermissionDto)
        {
            if (RolFormPermissionDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("RolFormPermission", "El rolFormPermission no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(RolFormPermissionDto.RolName) && string.IsNullOrWhiteSpace(RolFormPermissionDto.RolName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rolFormPermission con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rolFormPermission es obligatorio");
            }

        }








        // Método para mapear de RolFormPermission a RolFormPermissionDTO
        private RolFormPermissionDto MapToDTO(RolFormPermission rolFormPermission)
        {
            return new RolFormPermissionDto
            {
                Id = rolFormPermission.Id,
                RolId = rolFormPermission.RolId,
                FormId = rolFormPermission.FormId,
                PermissionId = rolFormPermission.PermissionId,
                //RolName = rolFormPermission.Rol.Name,
                //FormName = rolFormPermission.Form.Name,
                //PermissionName = rolFormPermission.Permission.Name,
                IsDeleted = rolFormPermission.IsDeleted

            };
        }

        // Método para mapear de RolFormPermissionDTO a RolFormPermission
        private RolFormPermission MapToEntity(RolFormPermissionDto rolFormPermissionDto)
        {
            return new RolFormPermission
            {
                Id = rolFormPermissionDto.Id,
                RolId = rolFormPermissionDto.RolId,
                FormId = rolFormPermissionDto.FormId,
                PermissionId = rolFormPermissionDto.PermissionId,
                IsDeleted = rolFormPermissionDto.IsDeleted


            };
        }

        // Método para mapear una lista de RolFormPermission a una lista de RolFormPermissionDTO
        private IEnumerable<RolFormPermissionDto> MapToDTOList(IEnumerable<RolFormPermission> rolFormPermissions)
        {
            var rolFormPermissionsDTO = new List<RolFormPermissionDto>();
            foreach (var rolFormPermission in rolFormPermissions)
            {
                rolFormPermissionsDTO.Add(MapToDTO(rolFormPermission));
            }
            return rolFormPermissionsDTO;
        }




    }
}
