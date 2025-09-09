using Business.Interfaces.Implements.Security;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Rols;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Web.Controllers.Implements.Security;

public class RolControllerTests
{
    private readonly RolController _controller;
    private readonly Mock<IRolService> _serviceMock;

    public RolControllerTests()
    {
        _serviceMock = new Mock<IRolService>();
        var loggerMock = new Mock<ILogger<RolController>>();
        _controller = new RolController(_serviceMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WithListOfRoles()
    {
        // Arrange
        var fakeData = new List<RolSelectDto>
        {
            new RolSelectDto { Id = 1, Name = "Admin" },
            new RolSelectDto { Id = 2, Name = "Producer" },
        };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(fakeData);

        // Act
        var actionResult = await _controller.Get();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        var payload = Assert.IsAssignableFrom<IEnumerable<RolSelectDto>>(ok.Value);
        Assert.Equal(2, payload.Count());
        Assert.Contains(payload, r => r.Name == "Admin");
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenFound()
    {
        // Arrange
        var rol = new RolSelectDto { Id = 10, Name = "QA" };
        _serviceMock.Setup(s => s.GetByIdAsync(10)).ReturnsAsync(rol);

        // Act
        var actionResult = await _controller.GetById(10);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        var payload = Assert.IsType<RolSelectDto>(ok.Value);
        Assert.Equal(10, payload.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNull()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((RolSelectDto?)null);

        // Act
        var actionResult = await _controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenServiceReturnsTrue()
    {
        _serviceMock.Setup(s => s.DeleteAsync(7)).ReturnsAsync(true);

        var actionResult = await _controller.Delete(7);

        Assert.IsType<OkObjectResult>(actionResult);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenServiceReturnsFalse()
    {
        _serviceMock.Setup(s => s.DeleteAsync(7)).ReturnsAsync(false);

        var actionResult = await _controller.Delete(7);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task Put_ShouldReturnOk_WhenUpdated()
    {
        var dto = new RolRegisterDto { Id = 5, Name = "Admin",Description= "Administa"};
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<RolRegisterDto>()))
                    .ReturnsAsync(true);

        var actionResult = await _controller.Put(5, dto);

        Assert.IsType<OkObjectResult>(actionResult);
    }

    [Fact]
    public async Task Put_ShouldReturnNotFound_WhenServiceReturnsFalse()
    {
        var dto = new RolRegisterDto { Id = 8, Name = "Admin", Description = "Administa" };
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<RolRegisterDto>()))
                    .ReturnsAsync(false);

        var actionResult = await _controller.Put(5, dto);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

}
