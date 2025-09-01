using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Orders;

namespace Data.Interfaces.Implements.Orders
{
    public interface IOrderRepository : IDataGeneric<Order>
    {
    }
}
