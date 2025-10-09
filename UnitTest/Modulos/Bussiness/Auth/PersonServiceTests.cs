using Business.Services.AuthService;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;
using MapsterMapper;
using Moq;
using Utilities.Exceptions;

namespace UnitTest.Modulos.Bussiness.Auth

{
    public class PersonServiceTests
    {
        private readonly Mock<IDataGeneric<Person>> _dataMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly PersonService _service;

        public PersonServiceTests()
        {
            _dataMock = new Mock<IDataGeneric<Person>>();
            _mapperMock = new Mock<IMapper>();
            _personRepositoryMock = new Mock<IPersonRepository>();

            _service = new PersonService(
                _dataMock.Object,
                _mapperMock.Object,
                _personRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            var people = new List<Person>
            {
                new() { Id = 1, FirstName = "Alice" },
                new() { Id = 2, FirstName = "Bob" }
            };
            var mapped = new List<PersonSelectDto>
            {
                new() { Id = 1, FullName = "Alice" },
                new() { Id = 2, FullName = "Bob" }
            };

            _dataMock.Setup(d => d.GetAllAsync()).ReturnsAsync(people);
            _mapperMock.Setup(m => m.Map<IEnumerable<PersonSelectDto>>(people)).Returns(mapped);

            var result = await _service.GetAllAsync();

            Assert.Collection(result,
                dto => Assert.Equal("Alice", dto.FullName),
                dto => Assert.Equal("Bob", dto.FullName));
            _dataMock.Verify(d => d.GetAllAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<PersonSelectDto>>(people), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowBusinessException_WhenIdIsInvalid()
        {
            await Assert.ThrowsAsync<BusinessException>(() => _service.GetByIdAsync(0));
            _dataMock.Verify(d => d.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldInitializeLogicalStateAndReturnMappedDto()
        {
            var dto = new PersonRegisterDto { FirstName = "Alice" };
            var mappedEntity = new Person { FirstName = "Alice", IsDeleted = true };
            var storedEntity = new Person { Id = 5, FirstName = "Alice", IsDeleted = false };
            var mappedBack = new PersonRegisterDto { Id = 5, FirstName = "Alice" };

            _mapperMock.Setup(m => m.Map<Person>(dto)).Returns(mappedEntity);
            _dataMock.Setup(d => d.AddAsync(It.IsAny<Person>())).ReturnsAsync(storedEntity);
            _mapperMock.Setup(m => m.Map<PersonRegisterDto>(storedEntity)).Returns(mappedBack);

            var result = await _service.CreateAsync(dto);

            Assert.Equal(5, result.Id);
            _dataMock.Verify(d => d.AddAsync(It.Is<Person>(p => p.FirstName == "Alice" && p.IsDeleted == false)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowBusinessException_WhenRepositoryFails()
        {
            _dataMock.Setup(d => d.DeleteAsync(10)).ThrowsAsync(new InvalidOperationException("db error"));

            await Assert.ThrowsAsync<BusinessException>(() => _service.DeleteAsync(10));
        }
    }
}