using Business.Interfaces.Implements.Auth;
using Entity.DTOs.Auth;
using Entity.DTOs.Auth.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Utilities.Exceptions;
using Web.Controllers.Implements.Auth;

namespace UnitTest.Auth;

public class UserControllerTests
{
    private readonly Mock<IUserService> _serviceMock = new();
    private readonly Mock<ILogger<UserController>> _loggerMock = new();

    private UserController CreateController()
    {
        return new UserController(_serviceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WithServiceData()
    {
        // Arrange
        var controller = CreateController();
        var expected = new List<UserSelectDto> { new() };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(expected);

        // Act
        var result = await controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task Get_ShouldReturnInternalServerError_WhenServiceThrows()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("failure"));

        // Act
        var result = await controller.Get();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenEntityExists()
    {
        // Arrange
        var controller = CreateController();
        var dto = new UserSelectDto();
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

        // Act
        var result = await controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenEntityMissing()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((UserSelectDto?)null);

        // Act
        var result = await controller.GetById(2);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }

    [Fact]
    public async Task GetById_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.GetByIdAsync(3))
            .ThrowsAsync(new ValidationException("invalid"));

        // Act
        var result = await controller.GetById(3);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnOk_WhenCreationSucceeds()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.CreateAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(new UserDto());

        var dto = new UserDto();

        // Act
        var result = await controller.Post(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.CreateAsync(It.IsAny<UserDto>()))
            .ThrowsAsync(new ValidationException("invalid"));

        // Act
        var result = await controller.Post(new UserDto());

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.CreateAsync(It.IsAny<UserDto>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await controller.Post(new UserDto());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenDtoDoesNotImplementBaseDto()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Put(5, new UserDto());

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<UserDto>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenRemovalSucceeds()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.DeleteAsync(6)).ReturnsAsync(true);

        // Act
        var result = await controller.Delete(6);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenServiceReturnsFalse()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.DeleteAsync(7)).ReturnsAsync(false);

        // Act
        var result = await controller.Delete(7);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.DeleteAsync(It.IsAny<int>()))
            .ThrowsAsync(new ValidationException("invalid"));

        // Act
        var result = await controller.Delete(8);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenEntityMissing()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.DeleteAsync(It.IsAny<int>()))
            .ThrowsAsync(new EntityNotFoundException("User", 9));

        // Act
        var result = await controller.Delete(9);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnInternalServerError_WhenExternalServiceFails()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.DeleteAsync(It.IsAny<int>()))
            .ThrowsAsync(new ExternalServiceException("external"));

        // Act
        var result = await controller.Delete(10);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.DeleteAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await controller.Delete(11);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task DeleteLogic_ShouldReturnOk_WhenSoftDeleteSucceeds()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.DeleteLogicAsync(12)).ReturnsAsync(true);

        // Act
        var result = await controller.DeleteLogic(12);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteLogic_ShouldReturnNotFound_WhenServiceReturnsFalse()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.DeleteLogicAsync(13)).ReturnsAsync(false);

        // Act
        var result = await controller.DeleteLogic(13);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }

    [Fact]
    public async Task DeleteLogic_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.DeleteLogicAsync(It.IsAny<int>()))
            .ThrowsAsync(new ValidationException("invalid"));

        // Act
        var result = await controller.DeleteLogic(14);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
    }

    [Fact]
    public async Task DeleteLogic_ShouldReturnNotFound_WhenEntityMissing()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.DeleteLogicAsync(It.IsAny<int>()))
            .ThrowsAsync(new EntityNotFoundException("User", 15));

        // Act
        var result = await controller.DeleteLogic(15);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }

    [Fact]
    public async Task DeleteLogic_ShouldReturnInternalServerError_WhenExternalServiceFails()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.DeleteLogicAsync(It.IsAny<int>()))
            .ThrowsAsync(new ExternalServiceException("external"));

        // Act
        var result = await controller.DeleteLogic(16);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task DeleteLogic_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.DeleteLogicAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await controller.DeleteLogic(17);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
}