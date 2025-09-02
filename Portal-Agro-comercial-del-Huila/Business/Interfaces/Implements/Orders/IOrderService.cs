using Business.Interfaces.IBusiness;
using Entity.Domain.Models.Implements.Orders;
using Entity.DTOs.Order.Create;
using Entity.DTOs.Order.Select;
using Entity.DTOs.Producer.Farm.Select;

namespace Business.Interfaces.Implements.Orders
{
    public interface IOrderService 
    {
        Task<OrderResultDto> CreateOrderAsync(int userId, OrderCreateDto dto);

        Task<IEnumerable<OrderSelectDto>> GetOrdersByProducer(int userId);
        Task<IEnumerable<OrderSelectDto>> GetPendingOrdersByProducer(int userId);
    }
}
