using System.Security.Claims;
using Business.Interfaces.Implements.Producers;
using Entity.DTOs.Auth;
using Entity.DTOs.Producer.Producer.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implements.Producer.Cuenta
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly IFarmService _farmService;
        public ProducerController(IFarmService farmService)
        {
            _farmService = farmService;
        }

        [HttpPost]
        [Authorize]
        [Route("Registrar/Producer")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Registrarse(ProducerWithFarmRegisterDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("El token no contiene un Claim 'sub' (NameIdentifier) válido o no es un ID.");

                var userCreated = await _farmService.RegisterWithProducer(dto,userId);

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { isSuccess = false, message = ex.Message });
            }
        }


    }
}
