using Business.Interfaces.Implements.Orders;
using Business.Interfaces.Implements.Producers.Cloudinary;
using Data.Interfaces.Implements.Orders;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Producers.Products;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Orders;
using Entity.DTOs.Order.Create;
using Entity.DTOs.Order.Select;
using Entity.DTOs.Producer.Farm.Select;
using Entity.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Utilities.Exceptions;

namespace Business.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IProductRepository _productRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly ApplicationDbContext _db;

        public OrderService(IMapper mapper, IOrderRepository orderRepository, ICloudinaryService cloudinaryService,
            IProductRepository productRepository, ApplicationDbContext db,IProducerRepository producerRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _cloudinaryService = cloudinaryService;
            _productRepository = productRepository;
            _db = db;
            _producerRepository = producerRepository;
        }

        public async Task<OrderResultDto> CreateOrderAsync(int userId, OrderCreateDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (dto.QuantityRequested <= 0) throw new BusinessException("La cantidad solicitada debe ser mayor a cero.");
            if (dto.PaymentImage is null) throw new BusinessException("Debes adjuntar el comprobante de pago.");
            if (string.IsNullOrWhiteSpace(dto.AddressLine1)) throw new BusinessException("La dirección es obligatoria.");

            var product = await _productRepository.GetByIdAsync(dto.ProductId)
                          ?? throw new BusinessException("Producto no encontrado.");
            if (!product.Active || product.IsDeleted)
                throw new BusinessException("El producto no está disponible.");

            var now = DateTime.UtcNow;

            var order = _mapper.Map<Order>(dto);
            order.UserId = userId;
            order.ProductId = product.Id;

            // snapshots del producto
            order.ProducerIdSnapshot = product.ProducerId;
            order.ProductNameSnapshot = product.Name;
            order.UnitPriceSnapshot = product.Price;

            order.Status = OrderStatus.PendingReview;

            order.Subtotal = product.Price * dto.QuantityRequested; // decimal * int => decimal
            order.DeliveryFee = 0m;
            order.DeliveryFeeCurrency = "COP";
            order.Total = order.Subtotal;
            order.AddressLine1 = dto.AddressLine1.Trim();

            order.CreateAt = now;

            string? uploadedPublicId = null; 

            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // Primer SaveChanges para obtener order.Id dentro de la misma transacción
                await _orderRepository.AddAsync(order);
                await _db.SaveChangesAsync();

                // Operación externa a la BD: si falla, hacemos rollback de la transacción.
                var upload = await _cloudinaryService.UploadOrderPaymentImageAsync(dto.PaymentImage, order.Id);
                order.PaymentImageUrl = upload.SecureUrl?.AbsoluteUri
                    ?? throw new BusinessException("No se pudo obtener la URL del comprobante.");
                order.PaymentUploadedAt = now;

                // Si tu servicio expone PublicId, úsalo para limpiar en caso de fallo posterior.
                uploadedPublicId = upload.PublicId;

                await _orderRepository.UpdateOrderAsync(order);
                await _db.SaveChangesAsync();

                await tx.CommitAsync();

                // Mapster convierte a DTO de salida
                return order.Adapt<OrderResultDto>();
            }
            catch (BusinessException)
            {
                // Fallo esperado de negocio: revertimos y re-lanzamos
                await tx.RollbackAsync();

                if (!string.IsNullOrEmpty(uploadedPublicId))
                {
                    try { await _cloudinaryService.DeleteAsync(uploadedPublicId); } catch { /* best-effort */ }
                }

                throw;
            }
            catch (Exception)
            {
                // Fallo inesperado: revertimos todo y entregamos mensaje controlado
                await tx.RollbackAsync();

                if (!string.IsNullOrEmpty(uploadedPublicId))
                {
                    try { await _cloudinaryService.DeleteAsync(uploadedPublicId); } catch { /* best-effort */ }
                }

                // aquí deberías loggear el detalle de la excepción
                throw new BusinessException("No se pudo crear la orden. Intenta de nuevo.");
            }
        }


        public async Task<IEnumerable<OrderSelectDto>> GetOrdersByProducer(int userId)
        {
            try
            {
                var producerId = await _producerRepository.GetIdProducer(userId)
                ?? throw new BusinessException("El usuario no está registrado como productor.");
                var entities = await _orderRepository.GetOrdersByProducer(producerId);
                return _mapper.Map<IEnumerable<OrderSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        public async Task<IEnumerable<OrderSelectDto>> GetPendingOrdersByProducer(int userId)
        {
            try
            {
                var producerId = await _producerRepository.GetIdProducer(userId)
                ?? throw new BusinessException("El usuario no está registrado como productor.");

                var entities = await _orderRepository.GetPendingOrdersByProducer(producerId);
                return _mapper.Map<IEnumerable<OrderSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }
    }
}
