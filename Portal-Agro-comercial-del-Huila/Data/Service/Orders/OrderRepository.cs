using Data.Interfaces.Implements.Orders;
using Data.Repository;
using Entity.Domain.Models.Implements.Orders;
using Entity.Infrastructure.Context;

namespace Data.Service.Orders
{
    public class OrderRepository : DataGeneric<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
