using Business.Interfaces.Implements.Orders;
using Business.Interfaces.Implements.Producers.Cloudinary;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Orders;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Producers.Products;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Orders;
using Entity.Domain.Models.Implements.Producers.Products;
using Entity.DTOs.Order.Create;
using Entity.DTOs.Order.Select;
using Entity.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Messaging.Interfaces;

namespace Business.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IProductRepository _productRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly IOrderEmailService _orderEmailService;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _db;

        public OrderService(
            IMapper mapper,
            ILogger<OrderService> logger,
            IOrderRepository orderRepository,
            ICloudinaryService cloudinaryService,
            IProductRepository productRepository,
            ApplicationDbContext db,
            IOrderEmailService orderEmailService,
            IUserRepository userRepository,
            IProducerRepository producerRepository)

        {
            _mapper = mapper;
            _logger = logger;
            _orderRepository = orderRepository;
            _cloudinaryService = cloudinaryService;
            _productRepository = productRepository;
            _orderEmailService = orderEmailService;
            _userRepository = userRepository;
            _db = db;
            _producerRepository = producerRepository;
        }

        public async Task<int> CreateOrderAsync(int userId, OrderCreateDto dto)
        {
            // 1) normalizar y validar DTO (evita basura/espacios)
            NormalizeCreateDto(dto);     // ← normaliza primero
            ValidateCreateDto(dto);      // ← valida después

            // 2) producto disponible
            var product = await GetAvailableProductAsync(dto.ProductId);

            // 3) construir entidad de orden
            var now = DateTime.UtcNow;
            var order = BuildOrderEntity(userId, dto, product, now);

            string? uploadedPublicId = null;

            // 4) persistir + subir comprobante en transacción
            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                await _orderRepository.AddAsync(order);
                await _db.SaveChangesAsync();

                var upload = await _cloudinaryService.UploadOrderPaymentImageAsync(dto.PaymentImage, order.Id);
                ApplyPaymentReceipt(order, upload, now, out uploadedPublicId);

                await _orderRepository.UpdateOrderAsync(order);
                await _db.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch (BusinessException)
            {
                await tx.RollbackAsync();
                await DeleteUploadedReceiptIfNeededAsync(uploadedPublicId);
                throw;
            }
            catch
            {
                await tx.RollbackAsync();
                await DeleteUploadedReceiptIfNeededAsync(uploadedPublicId);
                throw new BusinessException("No se pudo crear la orden. Intenta de nuevo.");
            }

            // 5) correos (fuera de la transacción; nunca deben tumbar la creación)
            await SendOrderCreatedEmailsSafelyAsync(order);

            return order.Id;
        }
        public async Task<IEnumerable<OrderListItemDto>> GetOrdersByProducerAsync(int userId)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var entities = await _orderRepository.GetOrdersByProducerAsync(producerId);
            return _mapper.Map<IEnumerable<OrderListItemDto>>(entities);
        }


        public async Task<IEnumerable<OrderListItemDto>> GetPendingOrdersByProducerAsync(int userId)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var entities = await _orderRepository.GetPendingOrdersByProducerAsync(producerId);
            return _mapper.Map<IEnumerable<OrderListItemDto>>(entities);
        }



        public async Task<OrderDetailDto> GetOrderDetailForProducerAsync(int userId, int orderId)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var order = await _orderRepository.GetByIdAsync(orderId)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");

            if (order.ProducerIdSnapshot != producerId)
                throw new BusinessException("No está autorizado para ver esta orden.");

            return _mapper.Map<OrderDetailDto>(order);
        }

        public async Task<OrderDetailDto> GetOrderDetailForUserAsync(int userId, int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");

            if (order.UserId != userId)
                throw new BusinessException("No está autorizado para ver esta orden.");

            return _mapper.Map<OrderDetailDto>(order);
        }

        public async Task AcceptOrderAsync(int userId, int orderId, OrderAcceptDto dto)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var order = await _orderRepository.GetByIdAsync(orderId)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");

            if (order.ProducerIdSnapshot != producerId)
                throw new BusinessException("No está autorizado para aceptar esta orden.");

            if (order.Status != OrderStatus.PendingReview)
                throw new BusinessException("Solo se pueden aceptar órdenes en estado pendiente.");

            if (string.IsNullOrWhiteSpace(order.PaymentImageUrl))
                throw new BusinessException("No se puede aceptar sin comprobante de pago.");

            // Concurrencia: RowVersion desde request (Base64 → byte[])
            order.RowVersion = Convert.FromBase64String(dto.RowVersion);

            // Aplicar aceptación (sin costos de envío)
            order.ProducerNotes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
            order.ProducerDecisionAt = DateTime.UtcNow;
            order.Status = OrderStatus.AcceptedAwaitingUser;

            try
            {
                await _orderRepository.UpdateOrderAsync(order);
                await _db.SaveChangesAsync();

                var user = await _userRepository.GetContactUser(order.UserId)
                    ?? throw new BusinessException("No se pudo obtener el contacto del usuario.");

                await _orderEmailService.SendOrderAcceptedToCustomer(
                    emailReceptor: user.Email,
                    orderId: order.Id,
                    productName: order.ProductNameSnapshot,
                    quantityRequested: order.QuantityRequested,
                    total: order.Total,
                    decisionAtUtc: order.ProducerDecisionAt!.Value
                );
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BusinessException("La orden fue modificada por otro usuario. Refresca y vuelve a intentar.");
            }
        }

        public async Task RejectOrderAsync(int userId, int orderId, OrderRejectDto dto)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var order = await _orderRepository.GetByIdAsync(orderId)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");

            if (order.ProducerIdSnapshot != producerId)
                throw new BusinessException("No está autorizado para rechazar esta orden.");

            if (order.Status != OrderStatus.PendingReview)
                throw new BusinessException("Solo se pueden rechazar órdenes en estado pendiente.");

            // Concurrencia
            order.RowVersion = Convert.FromBase64String(dto.RowVersion);

            // Rechazo
            order.ProducerDecisionAt = DateTime.UtcNow;
            order.ProducerDecisionReason = dto.Reason.Trim();
            order.Status = OrderStatus.Rejected;

            try
            {
                await _orderRepository.UpdateOrderAsync(order);
                await _db.SaveChangesAsync();

                var user = await _userRepository.GetContactUser(order.UserId)
                        ?? throw new BusinessException("No se pudo obtener el contacto del usuario.");

                await _orderEmailService.SendOrderRejectedToCustomer(
                    emailReceptor: user.Email,
                    orderId: order.Id,
                    productName: order.ProductNameSnapshot,
                    quantityRequested: order.QuantityRequested,
                    reason: order.ProducerDecisionReason!,
                    decisionAtUtc: order.ProducerDecisionAt!.Value
                );
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BusinessException("La orden fue modificada por otro usuario. Refresca y vuelve a intentar.");
            }
        }

        public async Task ConfirmOrderAsync(int userId, int orderId, OrderConfirmDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");

            if (order.UserId != userId)
                throw new BusinessException("No está autorizado para confirmar esta orden.");

            if (order.Status != OrderStatus.AcceptedAwaitingUser)
                throw new BusinessException("Solo se pueden confirmar órdenes aceptadas por el productor.");

            var decisionAt = order.ProducerDecisionAt
                             ?? throw new BusinessException("Orden inválida: falta la fecha de decisión del productor.");

            var enabledAt = order.UserConfirmEnabledAt ?? decisionAt.AddHours(48);
            if (DateTime.UtcNow < enabledAt)
                throw new BusinessException("Aún no está habilitada la confirmación de recepción.");

            // Concurrencia: RowVersion del request (Base64 -> byte[])
            order.RowVersion = Convert.FromBase64String(dto.RowVersion);

            var answer = dto.Answer.Trim().ToLowerInvariant();
            var now = DateTime.UtcNow;

            if (answer == "yes")
            {
                order.UserReceivedAnswer = UserReceivedAnswer.Yes;
                order.UserReceivedAt = now;
                order.Status = OrderStatus.Completed;
            }
            else if (answer == "no")
            {
                order.UserReceivedAnswer = UserReceivedAnswer.No;
                order.UserReceivedAt = now;
                order.Status = OrderStatus.Disputed;
            }
            else
            {
                throw new BusinessException("Answer debe ser 'Yes' o 'No'.");
            }

            try
            {
                await _orderRepository.UpdateOrderAsync(order);
                await _db.SaveChangesAsync();


                if (order.Status == OrderStatus.Completed) // answer == "yes"
                {
                    var producer = await _producerRepository.GetContactProducer(order.ProducerIdSnapshot)
                                   ?? throw new BusinessException("No se pudo obtener el contacto del productor.");

                    await _orderEmailService.SendOrderCompletedToProducer(
                        emailReceptor: producer.Email,
                        orderId: order.Id,
                        productName: order.ProductNameSnapshot,
                        quantityRequested: order.QuantityRequested,
                        total: order.Total,
                        completedAtUtc: order.UserReceivedAt!.Value
                    );
                }
                else if (order.Status == OrderStatus.Disputed) // answer == "no"
                {
                    var producer = await _producerRepository.GetContactProducer(order.ProducerIdSnapshot)
                                   ?? throw new BusinessException("No se pudo obtener el contacto del productor.");

                    await _orderEmailService.SendOrderDisputedToProducer(
                        emailReceptor: producer.Email,
                        orderId: order.Id,
                        productName: order.ProductNameSnapshot,
                        quantityRequested: order.QuantityRequested,
                        total: order.Total,
                        disputedAtUtc: order.UserReceivedAt!.Value
                    );
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BusinessException("La orden fue modificada por otro usuario. Refresca y vuelve a intentar.");
            }
        }


       


        // ===================== Helpers privados =====================

        // Normaliza campos de entrada (espacios, nulls)
        private static void NormalizeCreateDto(OrderCreateDto dto)
        {
            // Comentario: deja los strings en un formato coherente para validar y persistir
            dto.RecipientName = dto.RecipientName?.Trim() ?? string.Empty;
            dto.ContactPhone = dto.ContactPhone?.Trim() ?? string.Empty;
            dto.AddressLine1 = dto.AddressLine1?.Trim() ?? string.Empty;
            dto.AddressLine2 = string.IsNullOrWhiteSpace(dto.AddressLine2) ? null : dto.AddressLine2.Trim();
            dto.AdditionalNotes = string.IsNullOrWhiteSpace(dto.AdditionalNotes) ? null : dto.AdditionalNotes.Trim();
        }

        // Valida reglas básicas de creación
        private static void ValidateCreateDto(OrderCreateDto dto)
        {
            // Comentario: validaciones de negocio mínimas para crear una orden
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (dto.QuantityRequested <= 0) throw new BusinessException("La cantidad solicitada debe ser mayor a cero.");
            if (dto.PaymentImage is null) throw new BusinessException("Debes adjuntar el comprobante de pago.");
            if (string.IsNullOrWhiteSpace(dto.AddressLine1)) throw new BusinessException("La dirección es obligatoria.");
        }

        // Obtiene un producto activo/no eliminado (sino, lanza BusinessException)
        private async Task<Product> GetAvailableProductAsync(int productId)
        {
            // Comentario: garantiza que el producto existe y está disponible
            var product = await _productRepository.GetByIdAsync(productId)
                          ?? throw new BusinessException("Producto no encontrado.");

            if (!product.Active || product.IsDeleted)
                throw new BusinessException("El producto no está disponible.");

            return product;
        }

        // Construye la entidad Order completa (sin envío: Total = Subtotal)
        private static Order BuildOrderEntity(int userId, OrderCreateDto dto, Product product, DateTime now)
        {
            // Comentario: arma la orden con snapshots para inmutabilidad
            return new Order
            {
                UserId = userId,
                ProductId = product.Id,

                // Snapshots del producto
                ProducerIdSnapshot = product.ProducerId,
                ProductNameSnapshot = product.Name,
                UnitPriceSnapshot = product.Price,

                // Cantidad y totales (sin envío)
                QuantityRequested = dto.QuantityRequested,
                Subtotal = product.Price * dto.QuantityRequested,
                Total = product.Price * dto.QuantityRequested,

                // Estado inicial
                Status = OrderStatus.PendingReview,

                // Datos de entrega
                RecipientName = dto.RecipientName,
                ContactPhone = dto.ContactPhone,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                CityId = dto.CityId,
                AdditionalNotes = dto.AdditionalNotes,

                // Metadatos
                CreateAt = now,
                Active = true,
                IsDeleted = false
            };
        }

        // Aplica la evidencia de pago (URL + timestamps) a la orden
        private static void ApplyPaymentReceipt(Order order, dynamic upload, DateTime now, out string? uploadedPublicId)
        {
            // Comentario: asegura que tengamos la URL del comprobante y guardamos el PublicId por si hay que limpiar
            order.PaymentImageUrl = upload?.SecureUrl?.AbsoluteUri
                ?? throw new BusinessException("No se pudo obtener la URL del comprobante.");
            order.PaymentUploadedAt = now;
            uploadedPublicId = upload?.PublicId;
        }

        // Limpia en Cloudinary si subimos algo y luego falló la transacción
        private async Task DeleteUploadedReceiptIfNeededAsync(string? uploadedPublicId)
        {
            // Comentario: best-effort cleanup; nunca lanzar excepción aquí
            if (!string.IsNullOrEmpty(uploadedPublicId))
            {
                try { await _cloudinaryService.DeleteAsync(uploadedPublicId); }
                catch { /* ignore */ }
            }
        }

        // Envía los dos correos de "orden creada" sin romper el flujo si fallan
        private async Task SendOrderCreatedEmailsSafelyAsync(Order order)
        {
            try
            {
                // Comentario: resolver contactos (productor y usuario)
                var producer = await _producerRepository.GetContactProducer(order.ProducerIdSnapshot)
                               ?? throw new BusinessException("No se pudo obtener el contacto del productor.");

                var user = await _userRepository.GetContactUser(order.UserId)
                           ?? throw new BusinessException("No se pudo obtener el contacto del usuario.");

                // Comentario: construir nombres legibles (defensivo)
                string producerName = $"{producer.FirstName?.Trim()} {producer.LastName?.Trim()}".Trim();
                if (string.IsNullOrWhiteSpace(producerName)) producerName = "Productor";

                string customerName = $"{user.FirstName?.Trim()} {user.LastName?.Trim()}".Trim();
                if (string.IsNullOrWhiteSpace(customerName)) customerName = "Cliente";

                // Productor: “pendiente de revisión”
                await _orderEmailService.SendOrderCreatedEmail(
                    emailReceptor: producer.Email,
                    orderId: order.Id,
                    productName: order.ProductNameSnapshot,
                    quantityRequested: order.QuantityRequested,
                    subtotal: order.Subtotal,
                    total: order.Total,              // = Subtotal (sin envío)
                    createdAtUtc: order.CreateAt,
                    personName: producerName,
                    counterpartName: customerName,
                    isProducer: true
                );

                // Cliente: “recibimos tu pedido”
                await _orderEmailService.SendOrderCreatedEmail(
                    emailReceptor: user.Email,
                    orderId: order.Id,
                    productName: order.ProductNameSnapshot,
                    quantityRequested: order.QuantityRequested,
                    subtotal: order.Subtotal,
                    total: order.Total,
                    createdAtUtc: order.CreateAt,
                    personName: customerName,
                    counterpartName: producerName,
                    isProducer: false
                );
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed sending 'order created' emails (OrderId {OrderId})", order.Id);
            }
        }


    }
}
