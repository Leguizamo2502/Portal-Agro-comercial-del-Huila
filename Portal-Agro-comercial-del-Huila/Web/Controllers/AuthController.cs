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


        public AuthController(EncriptePassword utilities, ILogger<AuthController> logger, EncriptePassword utilidades, IAuthService authService)
        {
           
            _logger = logger;
            _utilities = utilities;
            _authService = authService; 

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


        //[HttpPost]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> Login([FromBody] LoginDto login)
        //{
        //    try
        //    {
        //        var token = await _token.GenerateToken(login);

        //        _ = _serviceEmail.WelcomeEmail(login.email);
        //        _ = _notifyManager.NotifyAsync();
        //        //Task.Run(() => _serviceEmail.EnviarEmailBienvenida(login.email));
        //        //await _serviceEmail.EnviarEmailBienvenida(login.email);

        //        return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token });

        //        //return Ok(token);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        _logger.LogWarning(ex, "Validación fallida para el inicio de sesión");
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (ExternalServiceException ex)
        //    {
        //        _logger.LogError(ex, "Error al crear el token");
        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("ValidarToken")]
        //public IActionResult ValidarToken([FromQuery] string token)

        //{

        //    bool respuesta = _token.validarToken(token);
        //    return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });

        //}

    }
}
