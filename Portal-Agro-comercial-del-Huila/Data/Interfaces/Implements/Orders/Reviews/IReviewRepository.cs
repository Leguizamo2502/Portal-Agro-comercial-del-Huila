using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Orders;

namespace Data.Interfaces.Implements.Orders.Reviews
{
    public interface IReviewRepository : IDataGeneric<Review>
    {
        Task<IEnumerable<Review>> GetAllByProductId(int productId);
    }
}
