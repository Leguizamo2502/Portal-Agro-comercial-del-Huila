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
    public class UserBusiness
    {
        private readonly UserData _userData;
        private readonly ILogger<UserBusiness> _logger;

        public UserBusiness(UserData userData, ILogger<UserBusiness> logger)
        {
            _userData = userData;
            _logger = logger;
        }

        // Método para obtener todos los usuarios como DTOs
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userData.GetAllAsync();
                var usersDTO = MapToDTOList(users);
                return usersDTO;

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
            }
        }

        // Método para obtener un usuario por ID como DTO
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intento obtener un usuario con un ID inválido: {UserId}", id);
                throw new InvalidDataException("ID de usuario inválido");
            }
            try
            {
                var user = await _userData.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogInformation("No se encontró el usuario con ID {UserId}", id);
                    throw new EntityNotFoundException("User",id);
                }
                return MapToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {UserId}", id);
                throw new ExternalServiceException("Base de datos", "Error al recuperar el usuario", ex);
            }
        }

        // Método para crear un usuario desde un DTO
        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            if (userDto == null)
            {
                _logger.LogWarning("Se intento crear un usuario nulo");
                throw new InvalidDataException("Usuario nulo");
            }
            try
            {
                ValidateUser(userDto);
                var user = MapToEntity(userDto);
                var newUser = await _userData.CreateAsync(user);
                return MapToDTO(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario {User}", userDto?.UserName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
            }
        }

        // Metodo para actualizar
        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            try
            {
                if (userDto == null || userDto.Id <= 0)
                {
                    throw new ValidationException("id", "El ID del user debe ser mayor que cero y no nulo");
                }

                var existingUser = await _userData.GetByIdAsync(userDto.Id);
                if (existingUser == null)
                {
                    throw new EntityNotFoundException("User", userDto.Id);
                }

                var updatedUser = MapToEntity(userDto);
                bool success = await _userData.UpdateAsync(updatedUser);

                if (!success)
                {
                    throw new Exception("No se pudo actualizar el user.");
                }

                return MapToDTO(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el user con ID {userDto?.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el user con ID {userDto?.Id}", ex);
            }
        }

        //Metodo para borrar logicamente
        public async Task<bool> DeleteUserLogicalAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del user debe ser mayor que cero");
                }

                var existingUser = await _userData.GetByIdAsync(id);
                if (existingUser == null)
                {
                    throw new EntityNotFoundException("User", id);
                }

                return await _userData.DeleteLogicAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al realizar la eliminación lógica del user con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al realizar la eliminación lógica del user con ID {id}", ex);
            }
        }

        //Metodo para borrar persistente
        public async Task<bool> DeleteUserPersistenceAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("id", "El ID del user debe ser mayor que cero");
                }

                var existingUser = await _userData.GetByIdAsync(id);
                if (existingUser == null)
                {
                    throw new EntityNotFoundException("User", id);
                }

                return await _userData.DeletePersistenceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar permanentemente el user con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el user con ID {id}", ex);
            }
        }

        //Metodo para validar el DTO
        public void ValidateUser(UserDto UserDto)
        {
            if (UserDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("User", "El user no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(UserDto.UserName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un user con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del user es obligatorio");
            }

        }

        // Método para mapear de User a UserDTO
        private UserDto MapToDTO(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Active = user.Active,
                IsDeleted = user.IsDeleted,
                PersonId = user.PersonId,
            };
        }

        // Método para mapear de UserDTO a User
        private User MapToEntity(UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                UserName = userDto.UserName,
                Active = userDto.Active,
                IsDeleted = userDto.IsDeleted,
                PersonId = userDto.PersonId,
                
            };
        }

        // Método para mapear una lista de User a una lista de UserDTO
        private IEnumerable<UserDto> MapToDTOList(IEnumerable<User> users)
        {
            var usersDTO = new List<UserDto>();
            foreach (var user in users)
            {
                usersDTO.Add(MapToDTO(user));
            }
            return usersDTO;
        }


    }
}
