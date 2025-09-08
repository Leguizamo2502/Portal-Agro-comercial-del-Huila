using Business.Interfaces.Implements.Orders;
using Entity.DTOs.Order.Create;
using Entity.DTOs.Order.Select;
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

        // POST: api/v1/order
        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(6_000_000)]
        public async Task<IActionResult> Create([FromForm] OrderCreateDto dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var orderId = await _orderService.CreateOrderAsync(userId, dto);
                return Ok(new { IsSuccess = true, Message = "Orden creada correctamente.", OrderId = orderId });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear orden");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // GET: api/v1/order  (pedidos del productor)
        [HttpGet]
        public async Task<IActionResult> GetAllForProducer()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                IEnumerable<OrderListItemDto> result = await _orderService.GetOrdersByProducerAsync(userId);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al listar órdenes del productor");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // GET: api/v1/order/pending  (pendientes del productor)
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingForProducer()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                IEnumerable<OrderListItemDto> result = await _orderService.GetPendingOrdersByProducerAsync(userId);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al listar órdenes pendientes del productor");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // GET: api/v1/order/{id}/for-producer  (detalle visible para el productor dueño)
        [HttpGet("{id:int}/for-producer")]
        public async Task<IActionResult> GetDetailForProducer(int id)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                OrderDetailDto dto = await _orderService.GetOrderDetailForProducerAsync(userId, id);
                return Ok(dto);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener detalle (producer)");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // GET: api/v1/order/{id}/for-user  (detalle visible para el cliente creador)
        [HttpGet("{id:int}/for-user")]
        public async Task<IActionResult> GetDetailForUser(int id)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                OrderDetailDto dto = await _orderService.GetOrderDetailForUserAsync(userId, id);
                return Ok(dto);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener detalle (user)");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }


        [HttpPost("{id:int}/accept")]
        public async Task<IActionResult> Accept(int id, [FromBody] OrderAcceptDto dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.AcceptOrderAsync(userId, id, dto);
                return Ok(new { IsSuccess = true, Message = "Pedido aceptado." });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al aceptar pedido");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        [HttpPost("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id, [FromBody] OrderRejectDto dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.RejectOrderAsync(userId, id, dto);
                return Ok(new { IsSuccess = true, Message = "Pedido rechazado." });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al rechazar pedido");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        [HttpPost("{id:int}/confirm-received")]
        public async Task<IActionResult> ConfirmReceived(int id, [FromBody] OrderConfirmDto dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.ConfirmOrderAsync(userId, id, dto);
                return Ok(new { IsSuccess = true, Message = "Confirmación registrada." });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al confirmar recepción");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }
    }
}
