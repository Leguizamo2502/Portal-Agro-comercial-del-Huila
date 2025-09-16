using Entity.DTOs.Order.Create;
using Entity.DTOs.Order.Select;

namespace Business.Interfaces.Implements.Orders
{
    public interface IOrderService 
    {
        Task<int> CreateOrderAsync(int userId, OrderCreateDto dto);

        Task<IEnumerable<OrderListItemDto>> GetOrdersByProducerAsync(int userId);
        Task<IEnumerable<OrderListItemDto>> GetPendingOrdersByProducerAsync(int userId);
        Task<OrderDetailDto> GetOrderDetailForProducerAsync(int userId, int orderId);
        Task<OrderDetailDto> GetOrderDetailForUserAsync(int userId, int orderId);
        Task AcceptOrderAsync(int userId, int orderId, OrderAcceptDto dto);
        Task RejectOrderAsync(int userId, int orderId, OrderRejectDto dto);
        Task ConfirmOrderAsync(int userId, int orderId, OrderConfirmDto dto);

        Task<IEnumerable<OrderListItemDto>> GetOrdersByUserAsync(int userId);
        Task CancelByUserAsync(int userId, int orderId, string rowVersionBase64);
        Task UploadPaymentAsync(int userId, int orderId, OrderUploadPaymentDto dto);
        Task MarkPreparingAsync(int userId, int orderId, string rowVersionBase64);
        Task MarkDispatchedAsync(int userId, int orderId, string rowVersionBase64);
        Task MarkDeliveredAsync(int userId, int orderId, string rowVersionBase64);



    }
}
