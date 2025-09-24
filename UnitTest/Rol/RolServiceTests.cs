using Business.Services.Security;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.IRepository;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Rols;
using MapsterMapper;
using Moq;
using Utilities.Exceptions;

namespace UnitTest.Rol
{
    public class RolServiceTests
    {
        private readonly Mock<IDataGeneric<Entity.Domain.Models.Implements.Security.Rol>> _dataMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRolRepository> _rolRepoMock;
        private readonly RolService _service;

        public RolServiceTests()
        {
            _dataMock = new Mock<IDataGeneric<Entity.Domain.Models.Implements.Security.Rol>>();
            _mapperMock = new Mock<IMapper>();
            _rolRepoMock = new Mock<IRolRepository>();

            _service = new RolService(_dataMock.Object, _mapperMock.Object, _rolRepoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            // Arrange
            var entities = new List<Entity.Domain.Models.Implements.Security.Rol>
        {
            new Entity.Domain.Models.Implements.Security.Rol { Id = 1, Name = "Admin", Description = "A" },
            new Entity.Domain.Models.Implements.Security.Rol { Id = 2, Name = "Producer", Description = "B" }
        };
            var mapped = new List<RolSelectDto>
        {
            new RolSelectDto { Id = 1, Name = "Admin" },
            new RolSelectDto { Id = 2, Name = "Producer" }
        };

            _dataMock.Setup(d => d.GetAllAsync()).ReturnsAsync(entities);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<RolSelectDto>>(entities))
                .Returns(mapped);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Name == "Admin");
            _dataMock.Verify(d => d.GetAllAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<RolSelectDto>>(entities), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenFound()
        {
            // Arrange
            var entity = new Entity.Domain.Models.Implements.Security.Rol { Id = 10, Name = "QA" };
            var dto = new RolSelectDto { Id = 10, Name = "QA" };

            _dataMock.Setup(d => d.GetByIdAsync(10)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<RolSelectDto>(entity)).Returns(dto);

            // Act
            var result = await _service.GetByIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result!.Id);
            _dataMock.Verify(d => d.GetByIdAsync(10), Times.Once);
            _mapperMock.Verify(m => m.Map<RolSelectDto>(entity), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _dataMock.Setup(d => d.GetByIdAsync(999)).ReturnsAsync((Entity.Domain.Models.Implements.Security.Rol?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
            _dataMock.Verify(d => d.GetByIdAsync(999), Times.Once);
            _mapperMock.Verify(m => m.Map<RolSelectDto>(It.IsAny<Entity.Domain.Models.Implements.Security.Rol>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldMap_CallAdd_AndReturnMappedDto()
        {
            // Arrange
            var input = new RolRegisterDto { Id = 0, Name = "New", Description = "Desc" };
            var mappedEntity = new Entity.Domain.Models.Implements.Security.Rol { Id = 0, Name = "New", Description = "Desc" };
            var createdEntity = new Entity.Domain.Models.Implements.Security.Rol { Id = 123, Name = "New", Description = "Desc" };
            var mappedBackDto = new RolRegisterDto { Id = 123, Name = "New", Description = "Desc" };

            _mapperMock.Setup(m => m.Map<Entity.Domain.Models.Implements.Security.Rol>(input)).Returns(mappedEntity);
            _dataMock.Setup(d => d.AddAsync(It.IsAny<Entity.Domain.Models.Implements.Security.Rol>())).ReturnsAsync(createdEntity);
            _mapperMock.Setup(m => m.Map<RolRegisterDto>(createdEntity)).Returns(mappedBackDto);

            // Act
            var result = await _service.CreateAsync(input);

            // Assert
            Assert.Equal(123, result.Id);
            Assert.Equal("New", result.Name);
            _mapperMock.Verify(m => m.Map<Entity.Domain.Models.Implements.Security.Rol>(input), Times.Once);
            _dataMock.Verify(d => d.AddAsync(It.Is<Entity.Domain.Models.Implements.Security.Rol>(r => r.Name == "New")), Times.Once);
            _mapperMock.Verify(m => m.Map<RolRegisterDto>(createdEntity), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            // Arrange
            var dto = new RolRegisterDto { Id = 5, Name = "Edit", Description = "E" };
            var entity = new Entity.Domain.Models.Implements.Security.Rol { Id = 5, Name = "Edit", Description = "E" };

            _mapperMock.Setup(m => m.Map<Entity.Domain.Models.Implements.Security.Rol>(dto)).Returns(entity);
            _dataMock.Setup(d => d.UpdateAsync(entity)).ReturnsAsync(true);

            // Act
            var ok = await _service.UpdateAsync(dto);

            // Assert
            Assert.True(ok);
            _mapperMock.Verify(m => m.Map<Entity.Domain.Models.Implements.Security.Rol>(dto), Times.Once);
            _dataMock.Verify(d => d.UpdateAsync(entity), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenRepositoryReturnsFalse()
        {
            // Arrange
            var dto = new RolRegisterDto { Id = 5, Name = "Edit", Description = "E" };
            var entity = new Entity.Domain.Models.Implements.Security.Rol { Id = 5, Name = "Edit", Description = "E" };

            _mapperMock.Setup(m => m.Map<Entity.Domain.Models.Implements.Security.Rol>(dto)).Returns(entity);
            _dataMock.Setup(d => d.UpdateAsync(entity)).ReturnsAsync(false);

            // Act
            var ok = await _service.UpdateAsync(dto);

            // Assert
            Assert.False(ok);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            _dataMock.Setup(d => d.DeleteAsync(7)).ReturnsAsync(true);

            var ok = await _service.DeleteAsync(7);

            Assert.True(ok);
            _dataMock.Verify(d => d.DeleteAsync(7), Times.Once);
        }

        [Fact]
        public async Task DeleteLogicAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            _dataMock.Setup(d => d.DeleteLogicAsync(7)).ReturnsAsync(true);

            var ok = await _service.DeleteLogicAsync(7);

            Assert.True(ok);
            _dataMock.Verify(d => d.DeleteLogicAsync(7), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowBusinessException_WhenIdIsZeroOrLess()
        {
            var ex = await Assert.ThrowsAsync<BusinessException>(() => _service.GetByIdAsync(0));
            Assert.Contains("Error al obtener el registro", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBusinessException_WhenDtoIsNull()
        {
            var ex = await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(null!));
            Assert.Contains("Error al crear el registro", ex.Message);
        }
    }
}
