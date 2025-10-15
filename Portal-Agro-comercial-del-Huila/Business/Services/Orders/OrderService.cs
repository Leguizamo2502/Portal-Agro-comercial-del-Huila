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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities.Custom.Code;
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
        private readonly int _paymentUploadDeadlineHours;
        private readonly int _deliveredConfirmDeadlineHours;

        public OrderService(
            IMapper mapper,
            ILogger<OrderService> logger,
            IOrderRepository orderRepository,
            ICloudinaryService cloudinaryService,
            IProductRepository productRepository,
            ApplicationDbContext db,
            IOrderEmailService orderEmailService,
            IUserRepository userRepository,
            IProducerRepository producerRepository,
            IConfiguration cfg)


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
            var hours = cfg.GetValue<int>("Orders:PaymentUploadDeadlineHours", 24);
            _paymentUploadDeadlineHours = Math.Clamp(hours, 1, 168);
            var hours2 = cfg.GetValue<int>("Orders:DeliveredConfirmDeadlineHours", 48);
            _deliveredConfirmDeadlineHours = Math.Clamp(hours2, 1, 336);

        }

        /// <summary>
        /// Creacion de orden sin comprobante
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<int> CreateOrderAsync(int userId, OrderCreateDto dto)
        {
            // 1) Normalizar y validar 
            NormalizeCreateDto(dto);
            ValidateCreateDto(dto);

            // 2) Producto disponible
            var product = await GetAvailableProductAsync(dto.ProductId);

            //validacion contra autopedido
            if (product.Producer != null && product.Producer.UserId == userId)
            {
                throw new BusinessException("No puedes comprar tus propios productos.");
            }

            // Validación de stock 
            if (product.Stock < dto.QuantityRequested)
                throw new BusinessException("Stock insuficiente para la cantidad solicitada.");

            // 3) Construcción de la entidad 
            var now = DateTime.UtcNow;
            var order = BuildOrderEntity(userId, dto, product, now);

            // 4) Persistencia 
            await _orderRepository.AddAsync(order);
            await _db.SaveChangesAsync();

            // 5) Notificaciones 
            await SendOrderCreatedEmailsSafelyAsync(order);

            return order.Id;
        }

     
        public async Task AcceptOrderAsync(int userId, string code, OrderAcceptDto dto)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var order = await _orderRepository.GetByCode(code)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");
            if (order.ProducerIdSnapshot != producerId)
                throw new BusinessException("No está autorizado para aceptar esta orden.");
            if (order.Status != OrderStatus.PendingReview)
                throw new BusinessException("Solo se pueden aceptar órdenes en estado pendiente.");

            // Concurrencia
            order.RowVersion = Convert.FromBase64String(dto.RowVersion);

            var now = DateTime.UtcNow;
            order.ProducerNotes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
            order.ProducerDecisionAt = now;
            order.Status = OrderStatus.AcceptedAwaitingPayment;
            order.AcceptedAt = now;
            order.AutoCloseAt = now.AddHours(_paymentUploadDeadlineHours);

            var strategy = _db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _db.Database.BeginTransactionAsync();
                try
                {
                    // 1) Intentar descontar stock (mismo DbContext/conn/tx)
                    var ok = await _productRepository.TryDecrementStockAsync(order.ProductId, order.QuantityRequested);
                    if (!ok)
                        throw new BusinessException("Stock insuficiente o concurrencia detectada. Refresca e inténtalo de nuevo.");

                    // 2) Persistir la orden
                    await _orderRepository.UpdateOrderAsync(order);
                    await _db.SaveChangesAsync();

                    await tx.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    await tx.RollbackAsync();
                    throw new BusinessException("La orden fue modificada por otro usuario. Refresca y vuelve a intentar.");
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });

            // Notificación fuera de la strategy/tx
            var user = await _userRepository.GetContactUser(order.UserId)
                      ?? throw new BusinessException("No se pudo obtener el contacto del usuario.");

            await _orderEmailService.SendOrderAcceptedAwaitingPaymentToCustomer(
                emailReceptor: user.Email,
                orderId: order.Id,
                productName: order.ProductNameSnapshot,
                quantityRequested: order.QuantityRequested,
                total: order.Total,
                acceptedAtUtc: order.AcceptedAt!.Value,
                paymentDeadlineUtc: order.AutoCloseAt!.Value
            );
        }


        public async Task UploadPaymentAsync(int userId, string code, OrderUploadPaymentDto dto)
        {
            var order = await _orderRepository.GetByCode(code)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");
            if (order.UserId != userId)
                throw new BusinessException("No está autorizado para subir el comprobante de esta orden.");
            if (order.Status != OrderStatus.AcceptedAwaitingPayment)
                throw new BusinessException("Solo se puede subir comprobante cuando la orden está aceptada y pendiente de pago.");
            if (order.AutoCloseAt.HasValue && DateTime.UtcNow > order.AutoCloseAt.Value)
                throw new BusinessException("El plazo para subir el comprobante expiró.");
            if (dto.PaymentImage == null || dto.PaymentImage.Length == 0)
                throw new BusinessException("Debes adjuntar el comprobante de pago.");

            // Concurrencia
            order.RowVersion = Convert.FromBase64String(dto.RowVersion);

            // === 1) IO externo: subir a Cloudinary FUERA de strategy/tx ===
            string? uploadedPublicId = null;
            string? uploadedUrl = null;
            var now = DateTime.UtcNow;

            try
            {
                var upload = await _cloudinaryService.UploadOrderPaymentImageAsync(dto.PaymentImage, order.Id);
                uploadedPublicId = upload?.PublicId;
                uploadedUrl = upload?.SecureUrl?.AbsoluteUri
                              ?? throw new BusinessException("No se pudo obtener la URL del comprobante.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo al subir comprobante (OrderId {OrderId})", order.Id);
                throw new BusinessException("No se pudo subir el comprobante. Intenta nuevamente.");
            }

            // === 2) Persistencia: strategy + transacción DENTRO ===
            var strategy = _db.Database.CreateExecutionStrategy();
            try
            {
                await strategy.ExecuteAsync(async () =>
                {
                    await using var tx = await _db.Database.BeginTransactionAsync();
                    try
                    {
                        order.PaymentImageUrl = uploadedUrl;
                        order.PaymentUploadedAt = now;
                        order.PaymentSubmittedAt = now;

                        order.Status = OrderStatus.PaymentSubmitted;
                        order.AutoCloseAt = null; // evita cierre automático

                        await _orderRepository.UpdateOrderAsync(order);
                        await _db.SaveChangesAsync();

                        await tx.CommitAsync();
                    }
                    catch
                    {
                        await tx.RollbackAsync();
                        throw;
                    }
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                await DeleteUploadedReceiptIfNeededAsync(uploadedPublicId);
                throw new BusinessException("La orden fue modificada por otro usuario. Refresca y vuelve a intentar.");
            }
            catch
            {
                await DeleteUploadedReceiptIfNeededAsync(uploadedPublicId);
                throw;
            }

            // === 3) Notificar (best-effort, fuera de strategy/tx) ===
            try
            {
                var producer = await _producerRepository.GetContactProducer(order.ProducerIdSnapshot)
                               ?? throw new BusinessException("No se pudo obtener el contacto del productor.");

                await _orderEmailService.SendPaymentSubmittedToProducer(
                    emailReceptor: producer.Email,
                    orderId: order.Id,
                    productName: order.ProductNameSnapshot,
                    quantityRequested: order.QuantityRequested,
                    total: order.Total,
                    uploadedAtUtc: order.PaymentUploadedAt!.Value
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed sending 'payment submitted' email (OrderId {OrderId})", order.Id);
            }
        }


        public async Task MarkPreparingAsync(int userId, string code, string rowVersionBase64)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var order = await _orderRepository.GetByCode(code)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");

            if (order.ProducerIdSnapshot != producerId)
                throw new BusinessException("No está autorizado para actualizar esta orden.");

            if (order.Status != OrderStatus.PaymentSubmitted)
                throw new BusinessException("Solo se puede pasar a 'Preparando' desde 'Pago enviado'.");

            // Concurrencia
            order.RowVersion = Convert.FromBase64String(rowVersionBase64);

            // Transición
            order.Status = OrderStatus.Preparing;

            try
            {
                await _orderRepository.UpdateOrderAsync(order);
                await _db.SaveChangesAsync();

                try
                {
                    var user = await _userRepository.GetContactUser(order.UserId)
                              ?? throw new BusinessException("No se pudo obtener el contacto del usuario.");
                    await _orderEmailService.SendOrderPreparingToCustomer(
                        emailReceptor: user.Email,
                        orderId: order.Id,
                        productName: order.ProductNameSnapshot,
                        quantityRequested: order.QuantityRequested,
                        total: order.Total,
                        preparingAtUtc: DateTime.UtcNow
                    );
                }
                catch (Exception exMail)
                {
                    _logger.LogError(exMail, "Error enviando email 'preparación' (OrderId {OrderId})", order.Id);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BusinessException("La orden fue modificada por otro usuario. Refresca y vuelve a intentar.");
            }
        }

        public async Task MarkDispatchedAsync(int userId, string code, string rowVersionBase64)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var order = await _orderRepository.GetByCode(code)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");

            if (order.ProducerIdSnapshot != producerId)
                throw new BusinessException("No está autorizado para actualizar esta orden.");

            if (order.Status != OrderStatus.Preparing)
                throw new BusinessException("Solo se puede pasar a 'Despachado' desde 'Preparando'.");

            order.RowVersion = Convert.FromBase64String(rowVersionBase64);
            order.Status = OrderStatus.Dispatched;

            try
            {
                await _orderRepository.UpdateOrderAsync(order);
                await _db.SaveChangesAsync();

                try
                {
                    var user = await _userRepository.GetContactUser(order.UserId)
                              ?? throw new BusinessException("No se pudo obtener el contacto del usuario.");
                    await _orderEmailService.SendOrderDispatchedToCustomer(
                        emailReceptor: user.Email,
                        orderId: order.Id,
                        productName: order.ProductNameSnapshot,
                        quantityRequested: order.QuantityRequested,
                        total: order.Total,
                        dispatchedAtUtc: DateTime.UtcNow
                    );
                }
                catch (Exception exMail)
                {
                    _logger.LogError(exMail, "Error enviando email 'despachado' (OrderId {OrderId})", order.Id);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BusinessException("La orden fue modificada por otro usuario. Refresca y vuelve a intentar.");
            }
        }

        public async Task MarkDeliveredAsync(int userId, string code, string rowVersionBase64)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var order = await _orderRepository.GetByCode(code)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");

            if (order.ProducerIdSnapshot != producerId)
                throw new BusinessException("No está autorizado para actualizar esta orden.");

            if (order.Status != OrderStatus.Dispatched)
                throw new BusinessException("Solo se puede marcar 'Entregado (pendiente de confirmación)' desde 'Despachado'.");

            order.RowVersion = Convert.FromBase64String(rowVersionBase64);

            // Transición + habilitar confirmación del comprador
            var now = DateTime.UtcNow;
            order.Status = OrderStatus.DeliveredPendingBuyerConfirm;
            order.UserConfirmEnabledAt = now;
            order.AutoCloseAt = now.AddHours(_deliveredConfirmDeadlineHours);

            // Nota: si más adelante quieres autocerrar tras N horas sin confirmación,
            // puedes reutilizar AutoCloseAt aquí y crear otro BackgroundService para completarla.
            // order.AutoCloseAt = now.AddHours(cfgHorasAutoCierreEntrega);

            try
            {
                await _orderRepository.UpdateOrderAsync(order);
                await _db.SaveChangesAsync();

                try
                {
                    var user = await _userRepository.GetContactUser(order.UserId)
                              ?? throw new BusinessException("No se pudo obtener el contacto del usuario.");
                    await _orderEmailService.SendOrderDeliveredToCustomer(
                        emailReceptor: user.Email,
                        orderId: order.Id,
                        productName: order.ProductNameSnapshot,
                        quantityRequested: order.QuantityRequested,
                        total: order.Total,
                        deliveredAtUtc: DateTime.UtcNow
                    );
                }
                catch (Exception exMail)
                {
                    _logger.LogError(exMail, "Error enviando email 'entregado-pendiente-confirmación' (OrderId {OrderId})", order.Id);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BusinessException("La orden fue modificada por otro usuario. Refresca y vuelve a intentar.");
            }
        }

        public async Task CancelByUserAsync(int userId, string code, string rowVersionBase64)
        {
            var order = await _orderRepository.GetByCode(code)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.UserId != userId) throw new BusinessException("No autorizado.");
            if (order.Status != OrderStatus.PendingReview)
                throw new BusinessException("Solo se puede cancelar antes de que el productor decida.");

            order.RowVersion = Convert.FromBase64String(rowVersionBase64);
            order.Status = OrderStatus.CancelledByUser;
            order.AutoCloseAt = null;

            try
            {
                await _orderRepository.UpdateOrderAsync(order);
                await _db.SaveChangesAsync();
                try
                {
                    var producer = await _producerRepository.GetContactProducer(order.ProducerIdSnapshot)
                                   ?? throw new BusinessException("No se pudo obtener el contacto del productor.");
                    await _orderEmailService.SendOrderCancelledByUserToProducer(
                        emailReceptor: producer.Email,
                        orderId: order.Id,
                        productName: order.ProductNameSnapshot,
                        quantityRequested: order.QuantityRequested,
                        cancelledAtUtc: DateTime.UtcNow
                    );
                }
                catch (Exception exMail)
                {
                    _logger.LogError(exMail, "Error enviando email 'cancelado por cliente' (OrderId {OrderId})", order.Id);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BusinessException("La orden fue modificada por otro usuario. Refresca y vuelve a intentar.");
            }
        }





        /// <summary>
        /// Rechzar orden
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task RejectOrderAsync(int userId, string code, OrderRejectDto dto)
        {
            var producerId = await _producerRepository.GetIdProducer(userId)
                             ?? throw new BusinessException("El usuario no está registrado como productor.");

            var order = await _orderRepository.GetByCode(code)
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


        /// <summary>
        /// Confirmar orden
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task ConfirmOrderAsync(int userId, string code, OrderConfirmDto dto)
        {
            var order = await _orderRepository.GetByCode(code)
                       ?? throw new BusinessException("Orden no encontrada.");

            if (order.IsDeleted || !order.Active)
                throw new BusinessException("La orden no está disponible.");

            if (order.UserId != userId)
                throw new BusinessException("No está autorizado para confirmar esta orden.");

            if (order.Status != OrderStatus.DeliveredPendingBuyerConfirm)
                throw new BusinessException("Solo se pueden confirmar órdenes que el productor marcó como entregadas.");

            var decisionAt = order.ProducerDecisionAt
                             ?? throw new BusinessException("Orden inválida: falta la fecha de decisión del productor.");

            //var enabledAt = order.UserConfirmEnabledAt ?? decisionAt.AddHours(48);
            //if (DateTime.UtcNow < enabledAt)
            //    throw new BusinessException("Aún no está habilitada la confirmación de recepción.");

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

                    // Nuevo: correo al cliente
                    var user = await _userRepository.GetContactUser(order.UserId)
                              ?? throw new BusinessException("No se pudo obtener el contacto del usuario.");

                    await _orderEmailService.SendOrderCompletedToCustomer(
                        emailReceptor: user.Email,
                        orderId: order.Id,
                        productName: order.ProductNameSnapshot,
                        quantityRequested: order.QuantityRequested,
                        total: order.Total,
                        completedAtUtc: order.UserReceivedAt!.Value,
                        autoCompleted: false
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
            dto.RecipientName = dto.RecipientName?.Trim() ?? string.Empty;
            dto.ContactPhone = dto.ContactPhone?.Trim() ?? string.Empty;
            dto.AddressLine1 = dto.AddressLine1?.Trim() ?? string.Empty;
            dto.AddressLine2 = string.IsNullOrWhiteSpace(dto.AddressLine2) ? null : dto.AddressLine2.Trim();
            dto.AdditionalNotes = string.IsNullOrWhiteSpace(dto.AdditionalNotes) ? null : dto.AdditionalNotes.Trim();
        }

        // Valida reglas básicas de creación
        private static void ValidateCreateDto(OrderCreateDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (dto.ProductId <= 0) throw new BusinessException("Producto inválido.");
            if (dto.QuantityRequested <= 0) throw new BusinessException("La cantidad solicitada debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(dto.RecipientName)) throw new BusinessException("El nombre del destinatario es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.ContactPhone)) throw new BusinessException("El teléfono de contacto es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.AddressLine1)) throw new BusinessException("La dirección es obligatoria.");
            if (dto.CityId <= 0) throw new BusinessException("La ciudad es obligatoria.");
        }

        // Obtiene un producto activo/no eliminado (sino, lanza BusinessException)
        private async Task<Product> GetAvailableProductAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                          ?? throw new BusinessException("Producto no encontrado.");

            if (!product.Active || product.IsDeleted)
                throw new BusinessException("El producto no está disponible.");

            return product;
        }

        // Construye la entidad Order completa (sin envío: Total = Subtotal)
        private static Order BuildOrderEntity(int userId, OrderCreateDto dto, Product product, DateTime now)
        {
            return new Order
            {
                UserId = userId,
                ProductId = product.Id,
                Code = CodeGenerator.Generate(),

                // Snapshots del producto (inmutables)
                ProducerIdSnapshot = product.ProducerId,
                ProductNameSnapshot = product.Name,
                UnitPriceSnapshot = product.Price,

                // Cantidad y totales (sin envío: Total = Subtotal)
                QuantityRequested = dto.QuantityRequested,
                Subtotal = product.Price * dto.QuantityRequested,
                Total = product.Price * dto.QuantityRequested,

                // Estado inicial del nuevo flujo
                Status = OrderStatus.PendingReview,

                // Datos de entrega
                RecipientName = dto.RecipientName,
                ContactPhone = dto.ContactPhone,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                CityId = dto.CityId,
                AdditionalNotes = dto.AdditionalNotes,

                // Metadatos base
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
