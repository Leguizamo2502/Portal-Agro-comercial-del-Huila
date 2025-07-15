using Business.CustomJwt;
using Business.Interfaces.Implements;
using Entity.DTOs.Auth;
using Entity.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Utilities.Custom;
using Utilities.Exceptions;

namespace Web.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        //private readonly IToken _token;
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly EncriptePassword _utilities;
        private readonly IToken _token;


        public AuthController(EncriptePassword utilities, ILogger<AuthController> logger, EncriptePassword utilidades, 
            IAuthService authService, IToken token)
        {
           
            _logger = logger;
            _utilities = utilities;
            _authService = authService;
            _token = token;

        }

        [HttpPost]
        [Route("Registrarse")]
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


        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto login)
        {
            try
            {
                var token = await _token.GenerateToken(login);

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token });
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


        //[HttpGet]
        //[Route("ValidarToken")]
        //public IActionResult ValidarToken([FromQuery] string token)

        //{

        //    bool respuesta = _token.validarToken(token);
        //    return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });

        //}

    }
}
