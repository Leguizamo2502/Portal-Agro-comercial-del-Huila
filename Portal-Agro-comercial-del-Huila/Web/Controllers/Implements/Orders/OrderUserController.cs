using Business.Interfaces.Implements.Orders;
using Entity.DTOs.Order.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using Utilities.Helpers.Auth;

namespace Web.Controllers.Implements.Orders
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // autenticación requerida; sin políticas ni roles aquí
    public class OrderUserController : ControllerBase
    {
        private readonly ILogger<OrderUserController> _logger;
        private readonly IOrderService _orderService;
        private readonly IOrderReadService _orderReadService;

        public OrderUserController(
            ILogger<OrderUserController> logger,
            IOrderService orderService,
            IOrderReadService orderReadService)
        {
            _logger = logger;
            _orderService = orderService;
            _orderReadService = orderReadService;
        }

        // POST: api/v1/orders/user
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

        // GET: api/v1/orders/user/mine
        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _orderReadService.GetOrdersByUserAsync(userId);
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

        // GET: api/v1/orders/user/{code}/detail
        [HttpGet("{code}/detail")]
        public async Task<IActionResult> GetDetailForUser(string code)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var dto = await _orderReadService.GetOrderDetailForUserAsync(userId, code);
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

        // POST: api/v1/orders/user/{code}/payment
        [HttpPost("{code}/payment")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10_000_000)]
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

        // POST: api/v1/orders/user/{code}/confirm-received
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

        // POST: api/v1/orders/user/{code}/cancel
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
    }
}
