using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements;
using Data.Interfaces.Implements;
using Data.Service;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Auth;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Utilities.Custom;
using Utilities.Exceptions;

namespace Business.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userData;
        private readonly IRolUserRepository _rolUserData;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly EncriptePassword _utilities;

        public AuthService(IUserRepository userData,ILogger<AuthService> logger, IRolUserRepository rolUserData, IMapper mapper, EncriptePassword utilities)
        {
            _logger = logger;
            _userData = userData;
            _rolUserData = rolUserData;
            _mapper = mapper;
            _utilities = utilities;
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            try
            {
                // Validar que el correo no esté registrado
                if (await _userData.ExistsByEmailAsync(dto.Email))
                    throw new Exception("Correo ya registrado");

                // Mapear DTO a entidades
                var person = _mapper.Map<Person>(dto);
                var user = _mapper.Map<User>(dto);

                // Encriptar contraseña
                user.Password = _utilities.EncripteSHA256(user.Password);

                // Asignar relación
                user.Person = person;

                // Guardar usuario
                await _userData.AddAsync(user);

                // Asignar rol por defecto
                await _rolUserData.AsignateRolDefault(user);

                // Recuperar el usuario con sus relaciones para el mapeo correcto
                var createduser = await _userData.GetByIdAsync(user.Id);
                if (createduser == null)
                    throw new BusinessException("Error interno: el usuario no pudo ser recuperado tras la creación.");

                return _mapper.Map<UserDto>(createduser);
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error si tienes logger inyectado
                throw new BusinessException($"Error en el registro del usuario: {ex.Message}", ex);
            }
        }





    }
}
