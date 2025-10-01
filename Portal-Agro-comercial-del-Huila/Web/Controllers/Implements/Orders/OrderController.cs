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
    [Authorize] // exige usuario autenticado para todo el controlador
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        // ==================== Cliente (buyer) ====================

        // POST: api/v1/order
        // Crear orden (ahora en JSON; SIN comprobante)
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
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

        // GET: api/v1/order/mine  (pedidos del cliente autenticado)
        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _orderService.GetOrdersByUserAsync(userId);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al listar pedidos del usuario");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // GET: api/v1/order/{id}/for-user  (detalle visible para el cliente)
        [HttpGet("{code}/for-user")]
        public async Task<IActionResult> GetDetailForUser(string code)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var dto = await _orderService.GetOrderDetailForUserAsync(userId, code);
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

        // POST: api/v1/order/{id}/payment (subir comprobante)  <<< NUEVO
        [HttpPost("{code}/payment")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10_000_000)] // 10MB, ajusta si quieres
        public async Task<IActionResult> UploadPayment(string code, [FromForm] OrderUploadPaymentDto dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.UploadPaymentAsync(userId, code, dto);
                return Ok(new { IsSuccess = true, Message = "Comprobante subido." });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al subir comprobante");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // POST: api/v1/order/{id}/confirm-received (Yes/No)
        [HttpPost("{code}/confirm-received")]
        public async Task<IActionResult> ConfirmReceived(string code, [FromBody] OrderConfirmDto dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.ConfirmOrderAsync(userId, code, dto);
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

        // POST: api/v1/order/{id}/cancel (cancelar por cliente si está PendingReview)  <<< NUEVO
        [HttpPost("{code}/cancel")]
        public async Task<IActionResult> CancelByUser(string code, [FromBody] string rowVersionBase64)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _orderService.CancelByUserAsync(userId, code, rowVersionBase64);
                return Ok(new { IsSuccess = true, Message = "Orden cancelada." });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al cancelar orden");
                return StatusCode(500, new { IsSuccess = false, Message = "Error inesperado.", Detail = ex.Message });
            }
        }

        // ==================== Productor (seller) ====================

        // GET: api/v1/order  (todas del productor)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllForProducer()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _orderService.GetOrdersByProducerAsync(userId);
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

        // GET: api/v1/order/pending  (pendientes de revisión del productor)
        [HttpGet("pending")]
        [Authorize]
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

        // GET: api/v1/order/{id}/for-producer  (detalle para productor dueño)
        [HttpGet("{code}/for-producer")]
        [Authorize]
        public async Task<IActionResult> GetDetailForProducer(string code)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                OrderDetailDto dto = await _orderService.GetOrderDetailForProducerAsync(userId, code);
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

        // POST: api/v1/order/{id}/accept
        [HttpPost("{code}/accept")]
        [Authorize]
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

        // POST: api/v1/order/{id}/reject
        [HttpPost("{code}/reject")]
        [Authorize]
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

        // POST: api/v1/order/{id}/preparing    
        [HttpPost("{code}/preparing")]
        [Authorize]
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

        // POST: api/v1/order/{id}/dispatched
        [HttpPost("{code}/dispatched")]
        [Authorize]
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

        // POST: api/v1/order/{id}/delivered
        [HttpPost("{code}/delivered")]
        [Authorize]
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
