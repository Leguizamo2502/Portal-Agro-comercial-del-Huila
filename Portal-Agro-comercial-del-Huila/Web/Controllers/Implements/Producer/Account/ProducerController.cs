using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implements.Producer.Cuenta
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProducerController : ControllerBase
    {
        
    }
}
