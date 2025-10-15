using Entity.DTOs.Order.Select;

namespace Business.Interfaces.Implements.Orders
{
    public interface IOrderReadService
    {
        Task<IEnumerable<OrderListItemDto>> GetOrdersByProducerAsync(int userId);
        Task<IEnumerable<OrderListItemDto>> GetPendingOrdersByProducerAsync(int userId);
        Task<OrderDetailDto> GetOrderDetailForProducerAsync(int userId, string code);
        Task<IEnumerable<OrderListItemDto>> GetOrdersByUserAsync(int userId);
        Task<OrderDetailDto> GetOrderDetailForUserAsync(int userId, string code);
    }
}
