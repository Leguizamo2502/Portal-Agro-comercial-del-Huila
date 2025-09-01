using Business.Interfaces.IBusiness;
using Entity.DTOs.Order.Create;
using Entity.DTOs.Order.Select;
using Entity.DTOs.Producer.Farm.Select;

namespace Business.Interfaces.Implements.Orders
{
    public interface IOrderService 
    {
        Task<OrderResultDto> CreateOrderAsync(int userId, OrderCreateDto dto);
    }
}
