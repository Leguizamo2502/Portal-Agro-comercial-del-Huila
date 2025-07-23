using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.Implements.Auth;
using Custom.Encripter;
using Data.Interfaces.Implements.Auth;
using Data.Service;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Auth;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Messaging.Implements;
using Utilities.Messaging.Interfaces;

namespace Business.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userData;
        private readonly IRolUserRepository _rolUserData;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly ISendCode _emailService;
        private readonly IPasswordResetCodeRepository _passwordResetRepo;

        public AuthService(IUserRepository userData,ILogger<AuthService> logger, IRolUserRepository rolUserData, IMapper mapper,
            ISendCode emailService, IPasswordResetCodeRepository passwordResetRepo)
        {
            _logger = logger;
            _userData = userData;
            _rolUserData = rolUserData;
            _mapper = mapper;

            _emailService = emailService;
            _passwordResetRepo = passwordResetRepo;
        }

        public async Task<IEnumerable<string>> GetRolesUserAsync(int idUser)
        {
            try
            {
                var roles = await _rolUserData.GetRolesUserAsync(idUser);
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles del usuario con ID {UserId}", idUser);
                throw new BusinessException("Error al obtener roles del usuario", ex);
            }
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
                user.Password = EncriptePassword.EncripteSHA256(user.Password);

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


        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await _userData.GetByEmailAsync(email)
                ?? throw new ValidationException("Correo no registrado");

            var code = new Random().Next(100000, 999999).ToString();

            var resetCode = new PasswordResetCode
            {
                Email = email,
                Code = code,
                Expiration = DateTime.UtcNow.AddMinutes(10)
            };

            await _passwordResetRepo.AddAsync(resetCode);
            await _emailService.SendRecoveryCodeEmail(email, code);
        }

        public async Task ResetPasswordAsync(ConfirmResetDto dto)
        {
            var record = await _passwordResetRepo.GetValidCodeAsync(dto.Email, dto.Code)
                ?? throw new ValidationException("Código inválido o expirado");

            var user = await _userData.GetByEmailAsync(dto.Email)
                ?? throw new ValidationException("Usuario no encontrado");

            user.Password = EncriptePassword.EncripteSHA256(dto.NewPassword);
            await _userData.UpdateAsync(user);

            record.IsUsed = true;
            await _passwordResetRepo.UpdateAsync(record);
        }






    }
}
