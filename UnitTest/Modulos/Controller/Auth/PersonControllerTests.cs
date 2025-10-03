using Business.Interfaces.Implements.Auth;
using Entity.DTOs.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Utilities.Exceptions;
using Web.Controllers.Implements.Auth;

namespace UnitTest.Auth;

public class PersonControllerTests
{
    private readonly Mock<IPersonService> _serviceMock = new();
    private readonly Mock<ILogger<PersonController>> _loggerMock = new();

    private PersonController CreateController()
    {
        return new PersonController(_serviceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WithServiceData()
    {
        // Arrange
        var controller = CreateController();
        var expected = new List<PersonSelectDto> { new() };
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
        var dto = new PersonSelectDto();
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

        // Act
        var result = await controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenEntityIsMissing()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((PersonSelectDto?)null);

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
            .ThrowsAsync(new ValidationException("Invalid"));

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
            .Setup(s => s.CreateAsync(It.IsAny<PersonRegisterDto>()))
            .ReturnsAsync(new PersonRegisterDto());

        var dto = new PersonRegisterDto();

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
            .Setup(s => s.CreateAsync(It.IsAny<PersonRegisterDto>()))
            .ThrowsAsync(new ValidationException("Invalid"));

        // Act
        var result = await controller.Post(new PersonRegisterDto());

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.CreateAsync(It.IsAny<PersonRegisterDto>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await controller.Post(new PersonRegisterDto());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldPropagateIdAndReturnOk_WhenUpdateSucceeds()
    {
        // Arrange
        var controller = CreateController();
        PersonRegisterDto? capturedDto = null;
        _serviceMock
            .Setup(s => s.UpdateAsync(It.IsAny<PersonRegisterDto>()))
            .Callback<PersonRegisterDto>(dto => capturedDto = dto)
            .ReturnsAsync(true);

        var dto = new PersonRegisterDto();

        // Act
        var result = await controller.Put(5, dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(5, capturedDto?.Id);
    }

    [Fact]
    public async Task Put_ShouldReturnNotFound_WhenUpdateReturnsFalse()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.UpdateAsync(It.IsAny<PersonRegisterDto>()))
            .ReturnsAsync(false);

        // Act
        var result = await controller.Put(7, new PersonRegisterDto());

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.UpdateAsync(It.IsAny<PersonRegisterDto>()))
            .ThrowsAsync(new ValidationException("invalid"));

        // Act
        var result = await controller.Put(8, new PersonRegisterDto());

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock
            .Setup(s => s.UpdateAsync(It.IsAny<PersonRegisterDto>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await controller.Put(9, new PersonRegisterDto());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenRemovalSucceeds()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.DeleteAsync(3)).ReturnsAsync(true);

        // Act
        var result = await controller.Delete(3);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenServiceReturnsFalse()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.DeleteAsync(4)).ReturnsAsync(false);

        // Act
        var result = await controller.Delete(4);

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
        var result = await controller.Delete(6);

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
            .ThrowsAsync(new EntityNotFoundException("Person", 10));

        // Act
        var result = await controller.Delete(10);

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
        var result = await controller.Delete(11);

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
        var result = await controller.Delete(12);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task DeleteLogic_ShouldReturnOk_WhenSoftDeleteSucceeds()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.DeleteLogicAsync(13)).ReturnsAsync(true);

        // Act
        var result = await controller.DeleteLogic(13);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteLogic_ShouldReturnNotFound_WhenServiceReturnsFalse()
    {
        // Arrange
        var controller = CreateController();
        _serviceMock.Setup(s => s.DeleteLogicAsync(14)).ReturnsAsync(false);

        // Act
        var result = await controller.DeleteLogic(14);

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
        var result = await controller.DeleteLogic(15);

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
            .ThrowsAsync(new EntityNotFoundException("Person", 16));

        // Act
        var result = await controller.DeleteLogic(16);

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
        var result = await controller.DeleteLogic(17);

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
        var result = await controller.DeleteLogic(18);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
}