using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class RolUserBusiness
    {
        private readonly RolUserData _rolUserData;
        private readonly ILogger<RolUserBusiness> _logger;

        public RolUserBusiness(RolUserData rolUserData, ILogger<RolUserBusiness> logger)
        {
            _rolUserData = rolUserData;
            _logger = logger;
        }

        // Obtener todos los RolUser como DTOs
        public async Task<IEnumerable<RolUserDto>> GetAllRolUserAsync()
        {
            try
            {
                var rolUsers = await _rolUserData.GetAllAsync();
                //return MapToDtoList(rolUsers);
                return rolUsers;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los rolUser");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de rolUser", ex);
            }
        }

        // Obtener un RolUser por ID como DTO
        public async Task<RolUserDto> GetRolUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rolUser con un ID inválido: {RolUserId}", id);
                throw new ValidationException("id", "El ID del rolUser debe ser mayor a 0");
            }

            try
            {
                var rolUser = await _rolUserData.GetByIdDtoAsync(id);
                if (rolUser == null)
                {
                    _logger.LogInformation("No se encontró ningún rolUser con el ID {RolUserId}", id);
                    throw new EntityNotFoundException("RolUser", id);
                }
                //return MapToDTO(rolUser);
                return rolUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rolUser con ID {RolUserId}", id);
                throw new ExternalServiceException("Base de datos", "Error al recuperar el rolUser", ex);
            }
        }

        // Crear un RolUser desde un DTO
        public async Task<RolUserDto> CreateRolUserAsync(RolUserDto rolUserDto)
        {
            try
            {
                ValidateRolUser(rolUserDto);
                var rolUser = MapToEntity(rolUserDto);

                var rolUserCreado = await _rolUserData.CreateAsync(rolUser);
                return MapToDTO(rolUserCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el rolUser: {RolUserId}", rolUserDto?.Id);
                throw new ExternalServiceException("Base de datos", "Error al crear el rolUser", ex);
            }
        }




        public async Task<UpdateRolUserDto> UpdateRolUserAsync(UpdateRolUserDto RolUserDto)
        {
            try
            {
                //ValidateRolUser(RolUserDto);
                if (RolUserDto == null || RolUserDto.Id <= 0)
                {
                    throw new ValidationException("id", "El ID del formModule debe ser mayor que cero y no nulo");
                }

                var existingRolUser = await _rolUserData.GetByIdAsync(RolUserDto.Id);

                if (existingRolUser == null)
                {
                    throw new EntityNotFoundException("RolUSer", RolUserDto.Id);
                }
                // existingForm = MapToEntity(RolUserDto);

                existingRolUser.RolId = RolUserDto.RolId;
                existingRolUser.UserId = RolUserDto.UserId;

                var update = await _rolUserData.UpdateLinQAsync(existingRolUser);

                if (!update)
                    throw new ExternalServiceException("Base de datos", "No se pudo actualizar el RolUser");


                return new UpdateRolUserDto
                {
                    Id = existingRolUser.Id,
                    RolId = existingRolUser.RolId,
                    UserId = existingRolUser.UserId
                   
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el RolUserDto con ID {RolUserDto?.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el RolUser con ID {RolUserDto?.Id}", ex);
            }
        }


        // Método para eliminar un Formulario Logicamente 
        public async Task<bool> DeleteFormLogicalAsync(int id)
        {
            ValidateId(id);

            try
            {
                var existingForm = await _rolUserData.GetByIdAsync(id);
                if (existingForm == null)
                    throw new EntityNotFoundException("RolUser", id);

                return await _rolUserData.DeleteLogicAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el RolUserDto con ID {RolUserId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el RolUser", ex);
            }
        }


        //  Método para eliminar un Formulario de manera persistente 
        public async Task<bool> DeleteFormPersistentAsync(int id)
        {
            ValidateId(id);

            try
            {
                var existingForm = await _rolUserData.GetByIdAsync(id);
                if (existingForm == null)
                    throw new EntityNotFoundException("Form", id);

                return await _rolUserData.DeletePersistenceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente el RolUser con ID {RolUserId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar permanentemente el RolUser", ex);
            }
        }




        // Validar DTO
        public void ValidateRolUser(RolUserDto rolUserDto)
        {
            if (rolUserDto == null)
                throw new ValidationException("RolUser", "El rolUser no puede ser nulo");

            if (rolUserDto.RolId <= 0)
                throw new ValidationException("RolId", "El RolId debe ser mayor a 0");

            if (rolUserDto.UserId <= 0)
                throw new ValidationException("UserId", "El UserId debe ser mayor a 0");
        }

        private void ValidateId(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID del Form debe ser mayor que cero");
        }

        // Mapear de RolUser a RolUserDTO
        private RolUserDto MapToDTO(RolUser rolUser)
        {
            return new RolUserDto
            {
                Id = rolUser.Id,
                RolId = rolUser.RolId,
                //RolName = rolUser.Rol.Name,
                UserId = rolUser.UserId,
                //UserName = rolUser.User.UserName,
                IsDeleted = rolUser.IsDeleted,
            };
        }

        // Mapear de RolUserDto a RolUser
        public static RolUser MapToEntity(RolUserDto dto)
        {
            return new RolUser
            {
                Id = dto.Id,
                RolId = dto.RolId,
                UserId = dto.UserId,
                IsDeleted = dto.IsDeleted
            };
        }

        // Método para mapear una lista de Form 
        private IEnumerable<RolUserDto> MapToDtoList(IEnumerable<RolUser> RolUsers)
        {
            var RolUserDto = new List<RolUserDto>();
            foreach (var RolUser in RolUsers)
            {
                RolUserDto.Add(MapToDTO(RolUser));
            }
            return RolUserDto;
        }


        //  


    }
}