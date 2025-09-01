using Business.Interfaces.Implements.Orders;
using Entity.DTOs.Order.Create;
using Microsoft.AspNetCore.Mvc;
using Utilities.Helpers.Auth;

namespace Web.Controllers.Implements.Orders
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(6_000_000)] // ~6MB
        public async Task<IActionResult> Create([FromForm] OrderCreateDto dto)
        {
            var userId = HttpContext.GetUserId();

            var result = await _orderService.CreateOrderAsync(userId, dto);

            return Ok(new { IsSuccess = true, message = "Orden creado correctamente." });
        }

    }
}
