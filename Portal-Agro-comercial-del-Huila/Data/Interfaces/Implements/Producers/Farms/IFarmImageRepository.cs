using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers.Farms;

namespace Data.Interfaces.Implements.Producers.Farms
{
    public interface IFarmImageRepository : IDataGeneric<FarmImage>
    {
        Task AddImages(List<FarmImage> images);
        Task<List<FarmImage>> GetByFarmIdAsync(int farmId);
        Task<bool> DeleteByPublicIdAsync(string publicId);
        Task<bool> DeleteLogicalByPublicIdAsync(string publicId);
    }
}
