using Business.Interfaces.Implements.Auth;
using Custom.Encripter;
using Data.Interfaces.Implements.Auth;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;
using Entity.DTOs.Auth.User;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers.Business;
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
        private readonly IPersonRepository _personRepository;

        public AuthService(IUserRepository userData,ILogger<AuthService> logger, IRolUserRepository rolUserData, IMapper mapper,
            ISendCode emailService, IPasswordResetCodeRepository passwordResetRepo,IPersonRepository personRepository)
        {
            _logger = logger;
            _userData = userData;
            _rolUserData = rolUserData;
            _mapper = mapper;
            _personRepository = personRepository;
            _emailService = emailService;
            _passwordResetRepo = passwordResetRepo;
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto, int userId)
        {
            try
            {
                if (dto is null) throw new ValidationException("Datos inválidos.");
                if (string.IsNullOrWhiteSpace(dto.CurrentPassword) || string.IsNullOrWhiteSpace(dto.NewPassword))
                    throw new ValidationException("Las contraseñas no pueden estar vacías.");

                if (!BusinessValidationHelper.IsValidPassword(dto.NewPassword))
                {
                    throw new BusinessException("Contraseña no valida");
                }

                if (dto.NewPassword == dto.CurrentPassword)
                    throw new ValidationException("La nueva contraseña no puede ser igual a la actual.");


                var user = await _userData.GetByIdAsync(userId)
                           ?? throw new ValidationException("Usuario no encontrado.");


                var hashedCurrent = EncriptePassword.EncripteSHA256(dto.CurrentPassword);
                if (!string.Equals(user.Password, hashedCurrent, StringComparison.Ordinal))
                    throw new ValidationException("Credenciales inválidas.");

                // Actualizar nueva contraseña
                user.Password = EncriptePassword.EncripteSHA256(dto.NewPassword);

                await _userData.UpdateAsync(user);


            }
            catch (ValidationException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al cambiar la contraseña.", ex);
            }
        }


        public async Task<UserSelectDto?> GetDataBasic(int userId)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(userId, "El ID debe ser mayor que cero.");

                var entity = await _userData.GetDataBasic(userId);
                var roles = await _rolUserData.GetRolesUserAsync(userId);
                if (entity == null) return null;
                var select = _mapper.Map<UserSelectDto>(entity);
                select.Roles = roles;
                return select;
                
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {userId}.", ex);
            }
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
                if (await _userData.ExistsByEmailAsync(dto.Email))
                    throw new Exception("Correo ya registrado");

                if (await _userData.ExistsByDocumentAsync(dto.Identification))
                    throw new Exception("Ya existe una persona con este numero de identificacion");

                var validPassword = BusinessValidationHelper.IsValidPassword(dto.Password);
                if (!validPassword)
                {
                    throw new BusinessException("Contraseña no valida");
                }

                var person = _mapper.Map<Person>(dto);
                var user = _mapper.Map<User>(dto);

                user.Password = EncriptePassword.EncripteSHA256(user.Password);

                user.Person = person;
                
                await _userData.AddAsync(user);

                await _rolUserData.AsignateRolDefault(user);

                // Recuperar el usuario con sus relaciones para el mapeo correcto
                var createduser = await _userData.GetByIdAsync(user.Id);
                if (createduser == null)
                    throw new BusinessException("Error interno: el usuario no pudo ser recuperado tras la creación.");

                return _mapper.Map<UserDto>(createduser);
            }
            catch (Exception ex)
            {
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

        public async Task<bool> UpdatePerson(PersonUpdateDto dto, int userId)
        {
            try
            {
                var person = await _personRepository.GetByUserIdAsync(userId)
                    ?? throw new ValidationException("Usuario no encontrado");

                _mapper.Map(dto, person);

                await _personRepository.UpdateAsync(person);

                return true;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar la persona.", ex);
            }
        }

    }
}
