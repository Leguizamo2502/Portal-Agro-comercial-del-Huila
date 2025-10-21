using Business.CustomJwt;
using Business.Interfaces.Implements.Auth;
using Business.Interfaces.Implements.Security.Mes;
using Entity.Domain.Models.Implements.Auth.Token;
using Entity.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Utilities.Exceptions;
using Utilities.Helpers.Auth;
using Web.Infrastructures;

namespace Web.Controllers.Implements.Auth
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IToken _token;
        private readonly IMeService _meService;
        private readonly JwtSettings _jwt;
        private readonly CookieSettings _cookieSettings;
        private readonly IAuthCookieFactory _cookieFactory;


        public AuthController(ILogger<AuthController> logger, 
            IAuthService authService, IToken token, IMeService me,
            IOptions<JwtSettings> jwtOptions,
            IOptions<CookieSettings> cookieOptions,
            IAuthCookieFactory cookieFactory)
        {
           
            _logger = logger;
            _authService = authService;
            _token = token;
            _meService = me;
            _jwt = jwtOptions.Value;
            _cookieSettings = cookieOptions.Value;
            _cookieFactory = cookieFactory;

        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Registrarse(RegisterUserDto objeto)
        {
            try
            {
                var userCreated = await _authService.RegisterAsync(objeto);

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { isSuccess = false, message = ex.Message });
            }
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto, CancellationToken ct)
        {
            try
            {
                var (access, refresh, csrf) = await _token.GenerateTokensAsync(dto);

                var now = DateTime.UtcNow;

                Response.Cookies.Append(
                    _cookieSettings.AccessTokenName,
                    access,
                    _cookieFactory.AccessCookieOptions(now.AddMinutes(_jwt.AccessTokenExpirationMinutes)));

                Response.Cookies.Append(
                    _cookieSettings.RefreshTokenName,
                    refresh,
                    _cookieFactory.RefreshCookieOptions(now.AddDays(_jwt.RefreshTokenExpirationDays)));

                Response.Cookies.Append(
                    _cookieSettings.CsrfCookieName,
                    csrf,
                    _cookieFactory.CsrfCookieOptions(now.AddDays(_jwt.RefreshTokenExpirationDays)));

                return Ok(new { isSuccess = true, message = "Login exitoso" });
            }
            catch (UnauthorizedAccessException)
            {
                // Mensaje controlado y status 401
                return Unauthorized(new { isSuccess = false, message = "Credenciales inválidas" });
            }
        }



        /// <summary>Renueva tokens (usa refresh cookie + comprobación CSRF double-submit).</summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            var refreshCookie = Request.Cookies[_cookieSettings.RefreshTokenName];
            if (string.IsNullOrWhiteSpace(refreshCookie))
                return Unauthorized();

            // Validación CSRF (double-submit: header debe igualar cookie)
            if (!Request.Headers.TryGetValue("X-XSRF-TOKEN", out var headerValue))
                return Forbid();

            var csrfCookie = Request.Cookies[_cookieSettings.CsrfCookieName];
            if (string.IsNullOrWhiteSpace(csrfCookie) || csrfCookie != headerValue)
                return Forbid();

            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var (newAccess, newRefresh) = await _token.RefreshAsync(refreshCookie, remoteIp);

            var now = DateTime.UtcNow;

            // Eliminar cookies anteriores con las MISMAS opciones (path/domain/samesite) para asegurar borrado
            Response.Cookies.Delete(_cookieSettings.AccessTokenName, _cookieFactory.AccessCookieOptions(now));
            Response.Cookies.Delete(_cookieSettings.RefreshTokenName, _cookieFactory.RefreshCookieOptions(now));

            // Escribir nuevas
            Response.Cookies.Append(
                _cookieSettings.AccessTokenName,
                newAccess,
                _cookieFactory.AccessCookieOptions(now.AddMinutes(_jwt.AccessTokenExpirationMinutes)));

            Response.Cookies.Append(
                _cookieSettings.RefreshTokenName,
                newRefresh,
                _cookieFactory.RefreshCookieOptions(now.AddDays(_jwt.RefreshTokenExpirationDays)));

            return Ok(new { isSuccess = true });
        }


        /// <summary>Logout: revoca refresh token y borra cookies.</summary>
        [HttpPost("logout")]
        [AllowAnonymous] // puede hacerse sin estar autenticado si solo borra cookies
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            var refreshCookie = Request.Cookies[_cookieSettings.RefreshTokenName];
            if (!string.IsNullOrWhiteSpace(refreshCookie))
            {
                await _token.RevokeRefreshTokenAsync(refreshCookie);
            }

            var now = DateTime.UtcNow;
            Response.Cookies.Delete(_cookieSettings.AccessTokenName, _cookieFactory.AccessCookieOptions(now));
            Response.Cookies.Delete(_cookieSettings.RefreshTokenName, _cookieFactory.RefreshCookieOptions(now));
            Response.Cookies.Delete(_cookieSettings.CsrfCookieName, _cookieFactory.CsrfCookieOptions(now));

            return Ok(new { message = "Sesión cerrada" });
        }

        /// <summary>Revoca el refresh token actual (si existe) y elimina la cookie.</summary>
        [HttpPost("revoke-token")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokeToken()
        {
            var refreshToken = Request.Cookies[_cookieSettings.RefreshTokenName];
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest(new { message = "No hay refresh token" });

            await _token.RevokeRefreshTokenAsync(refreshToken);

            var now = DateTime.UtcNow;
            Response.Cookies.Delete(_cookieSettings.RefreshTokenName, _cookieFactory.RefreshCookieOptions(now));

            return Ok(new { message = "Refresh token revocado" });
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            
            var userId = HttpContext.GetUserId();


            var currentUserDto = await _meService.GetAllDataMeAsync(userId);

            return Ok(currentUserDto);
        }





        [Authorize]
        [HttpGet("DataBasic")]
        public async Task<IActionResult> GetDataBasic()
        {

            var userId = HttpContext.GetUserId();

            try
            {
                var currentUserDto = await _authService.GetDataBasic(userId);
                return Ok(currentUserDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDataBasic falló para UserId={UserId}", userId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocurrió un error interno al procesar la solicitud."
                );
            }
        }

        [HttpPost("recover/send-code")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EnviarCodigoAsync([FromBody] RequestResetDto dto)
        {
            try
            {
                await _authService.RequestPasswordResetAsync(dto.Email);
                return Ok(new { isSuccess = true, message = "Código enviado al correo (si el email es válido)" });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida en solicitud de código de recuperación");
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Correo no encontrado para recuperación");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Fallo al enviar el correo de recuperación");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al enviar código de recuperación");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost("recover/confirm")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ConfirmarCodigo([FromBody] ConfirmResetDto dto)
        {
            try
            {
                await _authService.ResetPasswordAsync(dto);
                return Ok(new { isSuccess = true, message = "Contraseña actualizada" });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al confirmar código");
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Código o usuario no encontrado");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Fallo al actualizar la contraseña");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al confirmar código de recuperación");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPut("ChangePassword")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeMyPassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var userId = HttpContext.GetUserId();

            try
            {
                await _authService.ChangePasswordAsync(dto, userId);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error inesperado." });
            }
        }


        [HttpPut("updatePerson")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] PersonUpdateDto dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();

                var result = await _authService.UpdatePerson(dto, userId);

                if (result)
                    return Ok(new { message = "Información actualizada correctamente." });

                return BadRequest("No se pudo actualizar la información.");
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error de negocio al actualizar persona");
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar persona");
                return StatusCode(500, new { error = "Ocurrió un error inesperado." });
            }
        }






    }
}
