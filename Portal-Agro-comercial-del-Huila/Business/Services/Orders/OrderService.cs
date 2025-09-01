using Business.Interfaces.Implements.Orders;
using Business.Interfaces.Implements.Producers.Cloudinary;
using Data.Interfaces.Implements.Orders;
using Data.Interfaces.Implements.Producers.Products;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Orders;
using Entity.DTOs.Order.Create;
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
        private readonly ApplicationDbContext _db;

        public OrderService(IMapper mapper, IOrderRepository orderRepository, ICloudinaryService cloudinaryService,
            IProductRepository productRepository, ApplicationDbContext db)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _cloudinaryService = cloudinaryService;
            _productRepository = productRepository;
            _db = db;
        }

        public async Task<OrderResultDto> CreateOrderAsync(int userId, OrderCreateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId)
                          ?? throw new BusinessException("Producto no encontrado.");
            if (!product.Active || product.IsDeleted)
                throw new BusinessException("El producto no está disponible.");

            var now = DateTime.UtcNow;

            var order = dto.Adapt<Order>();
            order.UserId = userId;
            order.ProductId = product.Id;

            order.ProducerIdSnapshot = product.ProducerId;
            order.ProductNameSnapshot = product.Name;
            order.UnitPriceSnapshot = product.Price; // decimal

            order.Status = OrderStatus.PendingReview;

            order.Subtotal = product.Price * dto.QuantityRequested; // decimal
            order.DeliveryFee = 0m;
            order.DeliveryFeeCurrency = "COP";
            order.Total = order.Subtotal;

            order.CreateAt = now;

            await using var tx = await _db.Database.BeginTransactionAsync();

            await _orderRepository.AddAsync(order);

            var upload = await _cloudinaryService.UploadOrderPaymentImageAsync(dto.PaymentImage!, order.Id);
            order.PaymentImageUrl = upload.SecureUrl.AbsoluteUri;
            order.PaymentUploadedAt = now;

            await _orderRepository.UpdateAsync(order);
            await _db.SaveChangesAsync();

            await tx.CommitAsync();

            // Mapster ya hace las conversiones de salida
            return order.Adapt<OrderResultDto>();
        }
    }
}
