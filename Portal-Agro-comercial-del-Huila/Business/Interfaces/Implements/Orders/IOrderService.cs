using Entity.DTOs.Order.Create;
using Entity.DTOs.Order.Select;

namespace Business.Interfaces.Implements.Orders
{
    public interface IOrderService 
    {
        Task<int> CreateOrderAsync(int userId, OrderCreateDto dto);

        //Task<IEnumerable<OrderListItemDto>> GetOrdersByProducerAsync(int userId);
        //Task<IEnumerable<OrderListItemDto>> GetPendingOrdersByProducerAsync(int userId);
        //Task<OrderDetailDto> GetOrderDetailForProducerAsync(int userId, string code);
        //Task<IEnumerable<OrderListItemDto>> GetOrdersByUserAsync(int userId);
        //Task<OrderDetailDto> GetOrderDetailForUserAsync(int userId, string code);
        Task AcceptOrderAsync(int userId, string code, OrderAcceptDto dto);
        Task RejectOrderAsync(int userId, string code, OrderRejectDto dto);
        Task ConfirmOrderAsync(int userId, string code, OrderConfirmDto dto);

        Task CancelByUserAsync(int userId, string code, string rowVersionBase64);
        Task UploadPaymentAsync(int userId, string code, OrderUploadPaymentDto dto);
        Task MarkPreparingAsync(int userId, string code, string rowVersionBase64);
        Task MarkDispatchedAsync(int userId, string code, string rowVersionBase64);
        Task MarkDeliveredAsync(int userId, string code, string rowVersionBase64);



    }
}
