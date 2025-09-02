using Business.Interfaces.Implements.Orders;
using Entity.DTOs.Order.Create;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
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

        // GET: api/producers/me/orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = HttpContext.GetUserId(); // tu helper para obtener el ID
                var result = await _orderService.GetOrdersByProducer(userId);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // GET: api/producers/me/orders/pending
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _orderService.GetPendingOrdersByProducer(userId);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

    }
}
