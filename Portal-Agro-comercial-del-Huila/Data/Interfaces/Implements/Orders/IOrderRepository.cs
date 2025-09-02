using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Orders;

namespace Data.Interfaces.Implements.Orders
{
    public interface IOrderRepository : IDataGeneric<Order>
    {
        Task<bool> UpdateOrderAsync(Order entity);
        Task<IEnumerable<Order>> GetOrdersByProducer(int producerId);
        Task<IEnumerable<Order>> GetPendingOrdersByProducer(int producerId);
    }
}
