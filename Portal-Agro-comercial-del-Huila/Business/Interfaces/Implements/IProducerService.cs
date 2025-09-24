using Entity.DTOs.Producer.Producer.Select;
using Entity.DTOs.Producer.Producer.Update;

namespace Business.Interfaces.Implements
{
    public interface IProducerService 
    {
        Task<ProducerSelectDto?> GetByCodeProducer(string codeProducer);
        Task<int> SalesNumberByCode(string codeProducer);
        Task<string?> GetCodeProducer(int userId);
        Task<bool> UpdateProfileAsync(int userId, ProducerUpdateDto dto);
    }
}
