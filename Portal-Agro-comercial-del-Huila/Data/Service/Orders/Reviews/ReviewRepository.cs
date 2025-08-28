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

        private IQueryable<Review> BaseQuery()
        {
            return _dbSet
                .AsNoTracking()
                .Include(r => r.User)
                    .ThenInclude(u => u.Person)
                .AsSplitQuery(); 
        }

        public override async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await BaseQuery()
                .ToListAsync();
                
        }
        public override async Task<Review?> GetByIdAsync(int id)
        {
            return await BaseQuery()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetAllByProductId(int productId)
        {
            return await BaseQuery()
                .Where(r=>r.ProductId == productId)
                .ToListAsync();
        }
    }
}
