using Entity.DTOs.Producer.Producer.Select;

namespace Business.Interfaces.Implements
{
    public interface IProducerService 
    {
        Task<ProducerSelectDto?> GetByCodeProducer(string codeProducer);
    }
}
