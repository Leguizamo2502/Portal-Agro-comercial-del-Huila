using Data.Interfaces.Implements.Orders.Reviews;
using Data.Repository;
using Entity.Domain.Models.Implements.Orders;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Orders.Reviews
{
    public class ReviewRepository : DataGeneric<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _dbSet
                .Include(r => r.User)
                    .ThenInclude(u=>u.Person)
                .ToListAsync();
                
        }
    }
}
