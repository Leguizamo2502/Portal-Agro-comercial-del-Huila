using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Orders;

namespace Data.Interfaces.Implements.Orders
{
    public interface IOrderRepository : IDataGeneric<Order>
    {
        Task<bool> UpdateOrderAsync(Order entity);
        Task<IEnumerable<Order>> GetOrdersByProducerAsync(int producerId);
        Task<IEnumerable<Order>> GetPendingOrdersByProducerAsync(int producerId);
        // IOrderRepository
        Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);
        Task<Order?> GetByCode(string code);

    }
}
