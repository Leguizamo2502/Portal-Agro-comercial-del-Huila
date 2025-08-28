using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces.IBusiness;
using Entity.Domain.Models.Implements.Orders;
using Entity.DTOs.Order.Reviews;

namespace Business.Interfaces.Implements.Orders.Reviews
{
    public interface IReviewService : IBusiness<ReviewCreateDto,ReviewSelectDto>
    {
        Task<ReviewSelectDto> CreateReviewAsync(ReviewCreateDto dto, int userId);
        Task<IEnumerable<ReviewSelectDto>> GetAllByProductId(int productId);
    }
}
