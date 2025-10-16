using Business.Interfaces.Implements.Orders;
using Entity.DTOs.Order.Create;
using Entity.DTOs.Order.Select;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using Utilities.Helpers.Auth;

namespace Web.Controllers.Implements.Orders
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] 
    public class OrderProducerController : ControllerBase
    {
        private readonly ILogger<OrderProducerController> _logger;
        private readonly IOrderService _orderService;
        private readonly IOrderReadService _orderReadService;

        public OrderProducerController(
            ILogger<OrderProducerController> logger,
            IOrderService orderService,
            IOrderReadService orderReadService)
        {
            _logger = logger;
            _orderService = orderService;
            _orderReadService = orderReadService;
        }

        // GET: api/v1/orders/producer
        [HttpGet]
        public async Task<IActionResult> GetAllForProducer()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _orderReadService.GetOrdersByProducerAsync(userId);
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

        // GET: api/v1/orders/producer/pending
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingForProducer()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                IEnumerable<OrderListItemDto> result = await _orderReadService.GetPendingOrdersByProducerAsync(userId);
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

        // GET: api/v1/orders/producer/{code}/detail
        [HttpGet("{code}/detail")]
        public async Task<IActionResult> GetDetailForProducer(string code)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                OrderDetailDto dto = await _orderReadService.GetOrderDetailForProducerAsync(userId, code);
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

        // POST: api/v1/orders/producer/{code}/accept
        [HttpPost("{code}/accept")]
        public async Task<IActionResult> Accept(string code, [FromBody] OrderAcceptDto dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.AcceptOrderAsync(userId, code, dto);
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

        // POST: api/v1/orders/producer/{code}/reject
        [HttpPost("{code}/reject")]
        public async Task<IActionResult> Reject(string code, [FromBody] OrderRejectDto dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.RejectOrderAsync(userId, code, dto);
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

        // POST: api/v1/orders/producer/{code}/preparing
        [HttpPost("{code}/preparing")]
        public async Task<IActionResult> MarkPreparing(string code, [FromBody] string rowVersionBase64)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.MarkPreparingAsync(userId, code, rowVersionBase64);
                return Ok(new { IsSuccess = true, Message = "Orden en preparación." });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al marcar 'preparando'");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // POST: api/v1/orders/producer/{code}/dispatched
        [HttpPost("{code}/dispatched")]
        public async Task<IActionResult> MarkDispatched(string code, [FromBody] string rowVersionBase64)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.MarkDispatchedAsync(userId, code, rowVersionBase64);
                return Ok(new { IsSuccess = true, Message = "Orden despachada." });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al marcar 'despachado'");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // POST: api/v1/orders/producer/{code}/delivered
        [HttpPost("{code}/delivered")]
        public async Task<IActionResult> MarkDelivered(string code, [FromBody] string rowVersionBase64)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.MarkDeliveredAsync(userId, code, rowVersionBase64);
                return Ok(new { IsSuccess = true, Message = "Orden entregada (pendiente de confirmación del cliente)." });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al marcar 'entregado'");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }
    }
}
