using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Favorites;

namespace Data.Interfaces.Implements.Favorites
{
    public interface IFavoriteRepository : IDataGeneric<Favorite>
    {
        Task<bool> ExistsAsync(int userId, int productId);
        Task<Favorite?> GetByFavoriteAsync(int userId, int productId);
        Task<HashSet<int>> GetFavoriteProductIdsByUserAsync(int userId);
    }
}
