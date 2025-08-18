using Business.Interfaces.IBusiness;
using Entity.DTOs.Producer.Farm.Select;
using Microsoft.AspNetCore.Http;

namespace Business.Interfaces.Implements.Producers.Farms
{
    public interface IFarmImageService : IBusiness<FarmImageSelectDto, FarmImageSelectDto>
    {
        Task<List<FarmImageSelectDto>> GetImagesByFarmIdAsync(int farmId);
        Task<List<FarmImageSelectDto>> AddImagesAsync(int farmId, IFormFileCollection files);
        Task DeleteImageByIdAsync(int imageId);
        Task<bool> DeleteLogicalByPublicIdAsync(string publicId);
        Task DeleteImagesByPublicIdsAsync(List<string> publicIds);
    }
}
