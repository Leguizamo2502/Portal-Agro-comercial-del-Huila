using Data.Interfaces.Implements.Favorites;
using Data.Repository;
using Entity.Domain.Models.Implements.Favorites;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Favorites
{
    public class FavoriteRepository : DataGeneric<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(ApplicationDbContext context) : base(context)
        {
        }


        public async Task<bool> ExistsAsync(int userId, int productId)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);
        }


    }
}
