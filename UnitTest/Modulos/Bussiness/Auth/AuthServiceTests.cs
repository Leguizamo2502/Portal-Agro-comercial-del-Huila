using Business.Services.AuthService;
using Custom.Encripter;
using Data.Interfaces.Implements.Auth;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Auth;
using Entity.DTOs.Auth.User;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Utilities.Exceptions;
using Utilities.Messaging.Interfaces;

namespace UnitTest.Modulos.Bussiness.Auth
{
    public class AuthServiceTests   
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRolUserRepository> _rolUserRepositoryMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ISendCode> _emailServiceMock;
        private readonly Mock<IPasswordResetCodeRepository> _passwordResetRepositoryMock;
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _rolUserRepositoryMock = new Mock<IRolUserRepository>();
            _loggerMock = new Mock<ILogger<AuthService>>();
            _mapperMock = new Mock<IMapper>();
            _emailServiceMock = new Mock<ISendCode>();
            _passwordResetRepositoryMock = new Mock<IPasswordResetCodeRepository>();
            _personRepositoryMock = new Mock<IPersonRepository>();

            _service = new AuthService(
                _userRepositoryMock.Object,
                _loggerMock.Object,
                _rolUserRepositoryMock.Object,
                _mapperMock.Object,
                _emailServiceMock.Object,
                _passwordResetRepositoryMock.Object,
                _personRepositoryMock.Object);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldHashPassword_AndPersist()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "OldPass1",
                NewPassword = "NewPass2"
            };
            var storedUser = new User
            {
                Id = 5,
                Password = EncriptePassword.EncripteSHA256(dto.CurrentPassword)
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(storedUser);
            _userRepositoryMock.Setup(r => r.UpdateAsync(storedUser)).ReturnsAsync(true);

            await _service.ChangePasswordAsync(dto, 5);

            var expectedHash = EncriptePassword.EncripteSHA256(dto.NewPassword);
            _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => ReferenceEquals(u, storedUser) && u.Password == expectedHash)), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrowValidationException_WhenUserNotFound()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "OldPass1",
                NewPassword = "NewPass2"
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<ValidationException>(() => _service.ChangePasswordAsync(dto, 10));
            _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrowValidationException_WhenCurrentPasswordDoesNotMatch()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "WrongPass1",
                NewPassword = "NewPass2"
            };
            var storedUser = new User
            {
                Id = 8,
                Password = EncriptePassword.EncripteSHA256("Correct1")
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(storedUser);

            await Assert.ThrowsAsync<ValidationException>(() => _service.ChangePasswordAsync(dto, 8));
            _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldWrapUnexpectedErrors_InBusinessException()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "OldPass1",
                NewPassword = "NewPass2"
            };
            var storedUser = new User
            {
                Id = 6,
                Password = EncriptePassword.EncripteSHA256(dto.CurrentPassword)
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(6)).ReturnsAsync(storedUser);
            _userRepositoryMock.Setup(r => r.UpdateAsync(storedUser)).ThrowsAsync(new InvalidOperationException("db failure"));

            var ex = await Assert.ThrowsAsync<BusinessException>(() => _service.ChangePasswordAsync(dto, 6));
            Assert.Equal("Error al cambiar la contraseña.", ex.Message);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
        }

        [Fact]
        public async Task GetDataBasic_ShouldReturnMappedUserWithRoles()
        {
            var entity = new User { Id = 12, Email = "user@test.com" };
            var dto = new UserSelectDto { Id = 12, Email = entity.Email };
            var roles = new[] { "Admin", "Producer" };

            _userRepositoryMock.Setup(r => r.GetDataBasic(12)).ReturnsAsync(entity);
            _rolUserRepositoryMock.Setup(r => r.GetRolesUserAsync(12)).ReturnsAsync(roles);
            _mapperMock.Setup(m => m.Map<UserSelectDto>(entity)).Returns(dto);

            var result = await _service.GetDataBasic(12);

            Assert.NotNull(result);
            Assert.Equal(dto, result);
            Assert.Equal(roles, result!.Roles);
        }

        [Fact]
        public async Task GetDataBasic_ShouldReturnNull_WhenEntityNotFound()
        {
            _userRepositoryMock.Setup(r => r.GetDataBasic(77)).ReturnsAsync((User?)null);
            _rolUserRepositoryMock.Setup(r => r.GetRolesUserAsync(77)).ReturnsAsync(Array.Empty<string>());

            var result = await _service.GetDataBasic(77);

            Assert.Null(result);
            _mapperMock.Verify(m => m.Map<UserSelectDto>(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task GetDataBasic_ShouldThrowBusinessException_WhenIdIsZero()
        {
            var ex = await Assert.ThrowsAsync<BusinessException>(() => _service.GetDataBasic(0));
            Assert.Contains("Error al obtener el registro", ex.Message);
        }

        [Fact]
        public async Task GetRolesUserAsync_ShouldReturnRoles()
        {
            var expected = new[] { "Admin" };
            _rolUserRepositoryMock.Setup(r => r.GetRolesUserAsync(5)).ReturnsAsync(expected);

            var result = await _service.GetRolesUserAsync(5);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetRolesUserAsync_ShouldLogAndWrapErrors()
        {
            _rolUserRepositoryMock
                .Setup(r => r.GetRolesUserAsync(9))
                .ThrowsAsync(new InvalidOperationException("boom"));

            var ex = await Assert.ThrowsAsync<BusinessException>(() => _service.GetRolesUserAsync(9));
            Assert.Equal("Error al obtener roles del usuario", ex.Message);
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error al obtener roles del usuario con ID 9")),
                    It.Is<Exception>(err => err.Message == "boom"),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_AssignRole_AndReturnDto()
        {
            var dto = new RegisterUserDto
            {
                Email = "test@example.com",
                Password = "Password1",
                FirstName = "Test",
                LastName = "User",
                Identification = "123",
                PhoneNumber = "555",
                Address = "Street 123",
                CityId = 1
            };
            var person = new Person { FirstName = dto.FirstName };
            var newUser = new User { Password = dto.Password };
            var createdUser = new User { Id = 42, Email = dto.Email, Person = person };
            var expectedDto = new UserDto { Id = 42, Email = dto.Email };

            _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
            _userRepositoryMock.Setup(r => r.ExistsByDocumentAsync(dto.Identification)).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<Person>(dto)).Returns(person);
            _mapperMock.Setup(m => m.Map<User>(dto)).Returns(newUser);
            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync((User u) =>
            {
                u.Id = 42;
                return u;
            });
            _rolUserRepositoryMock.Setup(r => r.AsignateRolDefault(It.IsAny<User>())).ReturnsAsync(new RolUser());
            _userRepositoryMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(createdUser);
            _mapperMock.Setup(m => m.Map<UserDto>(createdUser)).Returns(expectedDto);

            var result = await _service.RegisterAsync(dto);

            var expectedHash = EncriptePassword.EncripteSHA256(dto.Password);
            _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Person == person && u.Password == expectedHash)), Times.Once);
            _rolUserRepositoryMock.Verify(r => r.AsignateRolDefault(It.Is<User>(u => u.Id == 42)), Times.Once);
            Assert.Equal(expectedDto, result);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowBusinessException_WhenEmailExists()
        {
            var dto = new RegisterUserDto
            {
                Email = "existing@example.com",
                Password = "Password1",
                Identification = "ID1"
            };

            _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<BusinessException>(() => _service.RegisterAsync(dto));
            Assert.Contains("Correo ya registrado", ex.Message);
        }

        [Fact]
        public async Task RequestPasswordResetAsync_ShouldPersistResetCode_AndSendEmail()
        {
            const string email = "reset@test.com";
            var user = new User { Id = 2, Email = email };
            PasswordResetCode? persistedCode = null;

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);
            _passwordResetRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<PasswordResetCode>()))
                .ReturnsAsync((PasswordResetCode code) =>
                {
                    persistedCode = code;
                    return code;
                });

            await _service.RequestPasswordResetAsync(email);

            Assert.NotNull(persistedCode);
            Assert.Equal(email, persistedCode!.Email);
            Assert.Equal(6, persistedCode.Code.Length);
            Assert.True(persistedCode.Expiration > DateTime.UtcNow);
            _emailServiceMock.Verify(e => e.SendRecoveryCodeEmail(email, persistedCode.Code), Times.Once);
        }

        [Fact]
        public async Task RequestPasswordResetAsync_ShouldThrowValidationException_WhenEmailUnknown()
        {
            _userRepositoryMock.Setup(r => r.GetByEmailAsync("missing@test.com")).ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<ValidationException>(() => _service.RequestPasswordResetAsync("missing@test.com"));
        }

        [Fact]
        public async Task ResetPasswordAsync_ShouldUpdatePassword_AndMarkCodeUsed()
        {
            var dto = new ConfirmResetDto
            {
                Email = "user@test.com",
                Code = "123456",
                NewPassword = "NewPass1"
            };
            var resetCode = new PasswordResetCode { Email = dto.Email, Code = dto.Code, Expiration = DateTime.UtcNow.AddMinutes(5) };
            var user = new User { Id = 3, Email = dto.Email };

            _passwordResetRepositoryMock.Setup(r => r.GetValidCodeAsync(dto.Email, dto.Code)).ReturnsAsync(resetCode);
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(true);
            _passwordResetRepositoryMock.Setup(r => r.UpdateAsync(resetCode)).ReturnsAsync(true);

            await _service.ResetPasswordAsync(dto);

            var expectedHash = EncriptePassword.EncripteSHA256(dto.NewPassword);
            _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => ReferenceEquals(u, user) && u.Password == expectedHash)), Times.Once);
            Assert.True(resetCode.IsUsed);
            _passwordResetRepositoryMock.Verify(r => r.UpdateAsync(resetCode), Times.Once);
        }

        [Fact]
        public async Task ResetPasswordAsync_ShouldThrowValidationException_WhenCodeInvalid()
        {
            var dto = new ConfirmResetDto { Email = "user@test.com", Code = "123456", NewPassword = "NewPass1" };
            _passwordResetRepositoryMock.Setup(r => r.GetValidCodeAsync(dto.Email, dto.Code)).ReturnsAsync((PasswordResetCode?)null);

            await Assert.ThrowsAsync<ValidationException>(() => _service.ResetPasswordAsync(dto));
            _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePerson_ShouldMapAndPersistChanges()
        {
            var dto = new PersonUpdateDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "123",
                CityId = 4,
                Address = "Main"
            };
            var person = new Person { Id = 7 };

            _personRepositoryMock.Setup(r => r.GetByUserIdAsync(7)).ReturnsAsync(person);
            _personRepositoryMock.Setup(r => r.UpdateAsync(person)).ReturnsAsync(true);
            _mapperMock
                .Setup(m => m.Map(dto, person))
                .Callback<PersonUpdateDto, Person>((src, dest) =>
                {
                    dest.FirstName = src.FirstName;
                    dest.LastName = src.LastName;
                });

            var result = await _service.UpdatePerson(dto, 7);

            Assert.True(result);
            _personRepositoryMock.Verify(r => r.UpdateAsync(person), Times.Once);
        }

        [Fact]
        public async Task UpdatePerson_ShouldThrowValidationException_WhenPersonNotFound()
        {
            var dto = new PersonUpdateDto();
            _personRepositoryMock.Setup(r => r.GetByUserIdAsync(11)).ReturnsAsync((Person?)null);

            await Assert.ThrowsAsync<ValidationException>(() => _service.UpdatePerson(dto, 11));
        }

        [Fact]
        public async Task UpdatePerson_ShouldThrowBusinessException_WhenUpdateFails()
        {
            var dto = new PersonUpdateDto();
            var person = new Person { Id = 5 };
            _personRepositoryMock.Setup(r => r.GetByUserIdAsync(5)).ReturnsAsync(person);
            _mapperMock.Setup(m => m.Map(dto, person));
            _personRepositoryMock.Setup(r => r.UpdateAsync(person)).ThrowsAsync(new InvalidOperationException("db"));

            var ex = await Assert.ThrowsAsync<BusinessException>(() => _service.UpdatePerson(dto, 5));
            Assert.Equal("Error al actualizar la persona.", ex.Message);
        }
    }
}