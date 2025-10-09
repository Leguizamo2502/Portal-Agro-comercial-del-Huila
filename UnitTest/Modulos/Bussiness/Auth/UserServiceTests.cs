using Business.Services.AuthService;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;
using Entity.DTOs.Auth.User;
using MapsterMapper;
using Moq;
using Utilities.Exceptions;

namespace UnitTest.Modulos.Bussiness.Auth
{
    public class UserServiceTests
    {
        private readonly Mock<IDataGeneric<User>> _dataMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRolUserRepository> _rolUserRepositoryMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _dataMock = new Mock<IDataGeneric<User>>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _rolUserRepositoryMock = new Mock<IRolUserRepository>();

            _service = new UserService(
                _dataMock.Object,
                _mapperMock.Object,
                _userRepositoryMock.Object,
                _rolUserRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldMapUsersAndAttachRoles()
        {
            var users = new List<User>
            {
                new() { Id = 1, Email = "alice@example.com" },
                new() { Id = 2, Email = "bob@example.com" }
            };

            var mappedFirst = new UserSelectDto { Id = 1, Email = "alice@example.com" };
            var mappedSecond = new UserSelectDto { Id = 2, Email = "bob@example.com" };

            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _mapperMock.SetupSequence(m => m.Map<UserSelectDto>(It.IsAny<User>()))
                .Returns(mappedFirst)
                .Returns(mappedSecond);
            _rolUserRepositoryMock.Setup(r => r.GetRolesUserAsync(1)).ReturnsAsync(new List<string> { "Admin" });
            _rolUserRepositoryMock.Setup(r => r.GetRolesUserAsync(2)).ReturnsAsync(new List<string> { "Viewer" });

            var result = (await _service.GetAllAsync()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(new[] { "Admin" }, result[0].Roles);
            Assert.Equal(new[] { "Viewer" }, result[1].Roles);

            _userRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<UserSelectDto>(It.Is<User>(u => u.Id == 1)), Times.Once);
            _mapperMock.Verify(m => m.Map<UserSelectDto>(It.Is<User>(u => u.Id == 2)), Times.Once);
            _rolUserRepositoryMock.Verify(r => r.GetRolesUserAsync(1), Times.Once);
            _rolUserRepositoryMock.Verify(r => r.GetRolesUserAsync(2), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldThrowBusinessException_WhenRepositoryFails()
        {
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new InvalidOperationException("db error"));

            await Assert.ThrowsAsync<BusinessException>(() => _service.GetAllAsync());
        }

        [Fact]
        public async Task CreateAsync_ShouldInitializeLogicalStateAndReturnMappedDto()
        {
            var dto = new UserDto { Email = "alice@example.com" };
            var mappedEntity = new User { Email = "alice@example.com", IsDeleted = true };
            var stored = new User { Id = 10, Email = "alice@example.com", IsDeleted = false };
            var mappedBack = new UserDto { Id = 10, Email = "alice@example.com" };

            _mapperMock.Setup(m => m.Map<User>(dto)).Returns(mappedEntity);
            _dataMock.Setup(d => d.AddAsync(It.IsAny<User>())).ReturnsAsync(stored);
            _mapperMock.Setup(m => m.Map<UserDto>(stored)).Returns(mappedBack);

            var result = await _service.CreateAsync(dto);

            Assert.Equal(10, result.Id);
            _dataMock.Verify(d => d.AddAsync(It.Is<User>(u => u.Email == "alice@example.com" && u.IsDeleted == false)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldWrapErrorsInBusinessException()
        {
            var dto = new UserDto { Id = 1, Email = "alice@example.com" };
            var mapped = new User { Id = 1, Email = "alice@example.com" };

            _mapperMock.Setup(m => m.Map<User>(dto)).Returns(mapped);
            _dataMock.Setup(d => d.UpdateAsync(mapped)).ThrowsAsync(new InvalidOperationException("db error"));

            await Assert.ThrowsAsync<BusinessException>(() => _service.UpdateAsync(dto));
        }
    }
}