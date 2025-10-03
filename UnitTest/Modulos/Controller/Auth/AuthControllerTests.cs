using System.Net;
using System.Security.Claims;
using Business.CustomJwt;
using Business.Interfaces.Implements.Auth;
using Business.Interfaces.Implements.Security.Mes;
using Entity.Domain.Models.Implements.Auth.Token;
using Entity.DTOs.Auth;
using Entity.DTOs.Security.Me;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Web.Controllers.Implements.Auth;
using Web.Infrastructures;

namespace UnitTest.Auth;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<IToken> _tokenMock = new();
    private readonly Mock<IMeService> _meServiceMock = new();
    private readonly Mock<IAuthCookieFactory> _cookieFactoryMock = new();

    public AuthControllerTests()
    {
        _cookieFactoryMock
            .Setup(f => f.AccessCookieOptions(It.IsAny<DateTimeOffset>()))
            .Returns<DateTimeOffset>(expires => new CookieOptions { HttpOnly = true, Path = "/", Expires = expires.UtcDateTime });

        _cookieFactoryMock
            .Setup(f => f.RefreshCookieOptions(It.IsAny<DateTimeOffset>()))
            .Returns<DateTimeOffset>(expires => new CookieOptions { HttpOnly = true, Path = "/", Expires = expires.UtcDateTime });

        _cookieFactoryMock
            .Setup(f => f.CsrfCookieOptions(It.IsAny<DateTimeOffset>()))
            .Returns<DateTimeOffset>(expires => new CookieOptions { HttpOnly = false, Path = "/", Expires = expires.UtcDateTime });
    }

    private AuthController CreateController()
    {
        var loggerMock = new Mock<ILogger<AuthController>>();
        var jwtOptions = Options.Create(new JwtSettings
        {
            AccessTokenExpirationMinutes = 30,
            RefreshTokenExpirationDays = 7
        });
        var cookieOptions = Options.Create(new CookieSettings
        {
            AccessTokenName = "access",
            RefreshTokenName = "refresh",
            CsrfCookieName = "csrf",
            Secure = false,
            SameSite = SameSiteMode.Lax
        });

        var controller = new AuthController(
            loggerMock.Object,
            _authServiceMock.Object,
            _tokenMock.Object,
            _meServiceMock.Object,
            jwtOptions,
            cookieOptions,
            _cookieFactoryMock.Object);

        var httpContext = new DefaultHttpContext();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controller;
    }

    [Fact]
    public async Task Registrarse_ShouldReturnOk_WhenServiceSucceeds()
    {
        // Arrange
        var controller = CreateController();
        _authServiceMock
            .Setup(s => s.RegisterAsync(It.IsAny<RegisterUserDto>()))
            .ReturnsAsync(new UserDto());

        // Act
        var result = await controller.Registrarse(new RegisterUserDto());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var successProperty = objectResult.Value?.GetType().GetProperty("isSuccess");
        Assert.True((bool?)successProperty?.GetValue(objectResult.Value));
    }

    [Fact]
    public async Task Registrarse_ShouldReturnBadRequest_WhenServiceThrows()
    {
        // Arrange
        var controller = CreateController();
        _authServiceMock
            .Setup(s => s.RegisterAsync(It.IsAny<RegisterUserDto>()))
            .ThrowsAsync(new Exception("error"));

        // Act
        var result = await controller.Registrarse(new RegisterUserDto());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldAppendCookies_WhenCredentialsAreValid()
    {
        // Arrange
        var controller = CreateController();
        _tokenMock
            .Setup(t => t.GenerateTokensAsync(It.IsAny<LoginUserDto>()))
            .ReturnsAsync(("access-token", "refresh-token", "csrf-token"));

        // Act
        var result = await controller.Login(new LoginUserDto(), CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var setCookieHeaders = controller.HttpContext.Response.Headers["Set-Cookie"];
        Assert.True(setCookieHeaders.Count >= 3, "Expected the three auth cookies to be appended.");
        _tokenMock.Verify(t => t.GenerateTokensAsync(It.IsAny<LoginUserDto>()), Times.Once);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenTokenServiceThrows()
    {
        // Arrange
        var controller = CreateController();
        _tokenMock
            .Setup(t => t.GenerateTokensAsync(It.IsAny<LoginUserDto>()))
            .ThrowsAsync(new UnauthorizedAccessException());

        // Act
        var result = await controller.Login(new LoginUserDto(), CancellationToken.None);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Refresh_ShouldReturnUnauthorized_WhenRefreshCookieMissing()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Refresh(CancellationToken.None);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Refresh_ShouldReturnForbid_WhenCsrfHeaderMissing()
    {
        // Arrange
        var controller = CreateController();
        controller.HttpContext.Request.Headers.Add("Cookie", "refresh=token");

        // Act
        var result = await controller.Refresh(CancellationToken.None);

        // Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task Refresh_ShouldRotateTokens_WhenAllChecksPass()
    {
        // Arrange
        var controller = CreateController();
        controller.HttpContext.Request.Headers.Add("Cookie", "refresh=old-refresh; csrf=csrf-token");
        controller.HttpContext.Request.Headers.Add("X-XSRF-TOKEN", "csrf-token");
        controller.HttpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

        _tokenMock
            .Setup(t => t.RefreshAsync("old-refresh", "127.0.0.1"))
            .ReturnsAsync(("new-access", "new-refresh"));

        // Act
        var result = await controller.Refresh(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var cookies = controller.HttpContext.Response.Headers["Set-Cookie"];
        Assert.True(cookies.Count >= 2);
        _tokenMock.Verify(t => t.RefreshAsync("old-refresh", "127.0.0.1"), Times.Once);
    }

    [Fact]
    public async Task Logout_ShouldRevokeRefreshToken_WhenCookiePresent()
    {
        // Arrange
        var controller = CreateController();
        controller.HttpContext.Request.Headers.Add("Cookie", "refresh=to-revoke");

        // Act
        var result = await controller.Logout();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        _tokenMock.Verify(t => t.RevokeRefreshTokenAsync("to-revoke"), Times.Once);
    }

    [Fact]
    public async Task RevokeToken_ShouldReturnBadRequest_WhenCookieMissing()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.RevokeToken();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RevokeToken_ShouldRevoke_WhenCookiePresent()
    {
        // Arrange
        var controller = CreateController();
        controller.HttpContext.Request.Headers.Add("Cookie", "refresh=valid");

        // Act
        var result = await controller.RevokeToken();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _tokenMock.Verify(t => t.RevokeRefreshTokenAsync("valid"), Times.Once);
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturnOk_WithUserData()
    {
        // Arrange
        var controller = CreateController();
        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "42") }, "Test");
        controller.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

        var expected = new UserMeDto();
        _meServiceMock.Setup(s => s.GetAllDataMeAsync(42)).ReturnsAsync(expected);

        // Act
        var result = await controller.GetCurrentUser();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(expected, okResult.Value);
    }

    [Fact]
    public async Task GetDataBasic_ShouldReturnServerError_WhenServiceThrows()
    {
        // Arrange
        var controller = CreateController();
        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "99") }, "Test");
        controller.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

        _authServiceMock.Setup(s => s.GetDataBasic(99)).ThrowsAsync(new Exception("boom"));

        // Act
        var result = await controller.GetDataBasic();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task EnviarCodigo_ShouldReturnOk_WhenServiceSucceeds()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.EnviarCodigoAsync(new RequestResetDto { Email = "demo@test.com" });

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _authServiceMock.Verify(s => s.RequestPasswordResetAsync("demo@test.com"), Times.Once);
    }

    [Fact]
    public async Task ConfirmarCodigo_ShouldReturnNotFound_WhenUnauthorized()
    {
        // Arrange
        var controller = CreateController();
        _authServiceMock
            .Setup(s => s.ResetPasswordAsync(It.IsAny<ConfirmResetDto>()))
            .ThrowsAsync(new UnauthorizedAccessException("nope"));

        // Act
        var result = await controller.ConfirmarCodigo(new ConfirmResetDto());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ChangeMyPassword_ShouldReturnNoContent_WhenServiceCompletes()
    {
        // Arrange
        var controller = CreateController();
        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "7") }, "Test");
        controller.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

        // Act
        var result = await controller.ChangeMyPassword(new ChangePasswordDto());

        // Assert
        Assert.IsType<NoContentResult>(result);
        _authServiceMock.Verify(s => s.ChangePasswordAsync(It.IsAny<ChangePasswordDto>(), 7), Times.Once);
    }
    [Fact]
    public async Task ChangeMyPassword_ShouldReturnValidationProblem_WhenModelStateInvalid()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Password", "Required");

        // Act
        var result = await controller.ChangeMyPassword(new ChangePasswordDto());

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

        var problem = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
        Assert.NotEmpty(problem.Errors);
        Assert.True(problem.Errors.ContainsKey("Password"));
    }


    [Fact]
    public async Task Update_ShouldReturnOk_WhenUpdateIsSuccessful()
    {
        // Arrange
        var controller = CreateController();
        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "13") }, "Test");
        controller.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

        _authServiceMock.Setup(s => s.UpdatePerson(It.IsAny<PersonUpdateDto>(), 13)).ReturnsAsync(true);

        // Act
        var result = await controller.Update(new PersonUpdateDto());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenServiceReturnsFalse()
    {
        // Arrange
        var controller = CreateController();
        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "13") }, "Test");
        controller.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

        _authServiceMock.Setup(s => s.UpdatePerson(It.IsAny<PersonUpdateDto>(), 13)).ReturnsAsync(false);

        // Act
        var result = await controller.Update(new PersonUpdateDto());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}