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
    public class PermissionBusiness
    {
        private readonly PermissionData _permissionData;
        private readonly ILogger<PermissionBusiness> _logger;

        public PermissionBusiness(PermissionData permissionData, ILogger<PermissionBusiness> logger)
        {
            _permissionData = permissionData;
            _logger = logger;
        }

        // Método para obtener todos los permisos com DTOs
        public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
        {
            try
            {
                var permissions = await _permissionData.GetAllAsync();
                var permissionsDTO = MapToDTOList(permissions);
                return permissionsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos", ex);
            }
        }

        // Método para obtener un permiso por ID como DTO

        public async Task<PermissionDto> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intento obtener un permiso con un ID inválido: {PermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("Permiso", "ID permiso debe ser mayor a 0");
            }
            try
            {
                var permission = await _permissionData.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el permiso con ID {PermissionId}", id);
                    throw new EntityNotFoundException("Permiso", "No se encontró el permiso");
                }
                return MapToDTO(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", "Error al recuperar el permiso", ex);
            }
        }

        //Metodo para crear un permiso desde un DTO
        public async Task<PermissionDto> CreatePermissionAsync(PermissionDto permissionDto)
        {
            
            try
            {
                ValidatePermission(permissionDto);
                var permission = MapToEntity(permissionDto);
                permission = await _permissionData.CreateAsync(permission);
                return MapToDTO(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el permiso {PermissionName}",permissionDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
            }
        }

        // Metodo para actualizar
        public async Task<PermissionDto> UpdatePermissionAsync(PermissionDto permissionDto)
        {
            try
            {
                if (permissionDto == null || permissionDto.Id <= 0)
                {
                    throw new ValidationException("id", "El ID del permission debe ser mayor que cero y no nulo");
                }

                var existingPermission = await _permissionData.GetByIdAsync(permissionDto.Id);
                if (existingPermission == null)
                {
                    throw new EntityNotFoundException("Permission", permissionDto.Id);
                }

                var updatedPermission = MapToEntity(permissionDto);
                bool success = await _permissionData.UpdateAsync(updatedPermission);

                if (!success)
                {
                    throw new Exception("No se pudo actualizar el permission.");
                }

                return MapToDTO(updatedPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el permission con ID {permissionDto?.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el permission con ID {permissionDto?.Id}", ex);
            }
        }

        //Metodo para borrar logicamente
        public async Task<bool> DeletePermissionLogicalAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del permission debe ser mayor que cero");
                }

                var existingPermission = await _permissionData.GetByIdAsync(id);
                if (existingPermission == null)
                {
                    throw new EntityNotFoundException("Permission", id);
                }

                return await _permissionData.DeleteLogicAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al realizar la eliminación lógica del permission con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al realizar la eliminación lógica del permission con ID {id}", ex);
            }
        }

        //Metodo para borrar persistente
        public async Task<bool> DeletePermissionPersistenceAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del permission debe ser mayor que cero");
                }

                var existingPermission = await _permissionData.GetByIdAsync(id);
                if (existingPermission == null)
                {
                    throw new EntityNotFoundException("Permission", id);
                }

                return await _permissionData.DeletePersistenceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar permanentemente el permission con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el permission con ID {id}", ex);
            }
        }


        //Metodo para validar DTO
        public void ValidatePermission(PermissionDto PermissionDto)
        {
            if(PermissionDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("Permiso", "El permiso no puede ser nulo");
            }
            if(string.IsNullOrWhiteSpace(PermissionDto.Name))
            {
                _logger.LogWarning("Se intento crear un permiso con nombre nulo o vacío");
                throw new Utilities.Exceptions.ValidationException("Nombre", "El nombre del permiso no puede ser nulo o vacío");
            }
        }

        //Metodo para mapear de permission a permissionDto
        private PermissionDto MapToDTO(Permission permission)
        {
            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                IsDeleted = permission.IsDeleted,
            };
        }

        //Metodo para mapear de permissionDto a permission
        private Permission MapToEntity(PermissionDto permissionDto)
        {
            return new Permission
            {
                Id = permissionDto.Id,
                Name = permissionDto.Name,
                Description = permissionDto.Description,
                IsDeleted = permissionDto.IsDeleted,
            };
        }

        //Metodo par mapear una lista de permisos a una lista de permisosDTO
        private IEnumerable<PermissionDto> MapToDTOList(IEnumerable<Permission> permissions)
        {
            var permissionsDTO = new List<PermissionDto>();
            foreach (var permission in permissions)
            {
                permissionsDTO.Add(MapToDTO(permission));
            }
            return permissionsDTO;
        }




    }
}
