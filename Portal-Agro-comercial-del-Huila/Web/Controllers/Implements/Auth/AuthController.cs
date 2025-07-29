using System.Security.Claims;
using Business.CustomJwt;
using Business.Interfaces.Implements.Auth;
using Business.Interfaces.Implements.Location;
using Business.Interfaces.Implements.Security.Mes;
using Entity.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

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


        public AuthController(ILogger<AuthController> logger, 
            IAuthService authService, IToken token, IMeService me)
        {
           
            _logger = logger;
            _authService = authService;
            _token = token;
            _meService = me;

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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> Login([FromBody] LoginUserDto login)
        {
            try
            {
                var token = await _token.GenerateToken(login);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // asegúrate que esto se respete en producción
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("jwt", token, cookieOptions);

                return Ok(new { isSuccess = true }); // ya no mandes el token
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Intento de inicio de sesión con credenciales inválidas");
                return Unauthorized(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el inicio de sesión");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el token");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en el login");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }



        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("El token no contiene un Claim 'sub' (NameIdentifier) válido o no es un ID.");

            var currentUserDto = await _meService.GetAllDataMeAsync(userId);

            return Ok(currentUserDto);
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "Sesión cerrada" });
        }



        //[HttpGet]
        //[Route("ValidarToken")]
        //public IActionResult ValidarToken([FromQuery] string token)

        //{

        //    bool respuesta = _token.validarToken(token);
        //    return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });

        //}

        [HttpPost("recuperar/enviar-codigo")]
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

        [HttpPost("recuperar/confirmar")]
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


        

        



    }
}
