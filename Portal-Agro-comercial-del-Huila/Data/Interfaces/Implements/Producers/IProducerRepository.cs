using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers;
using Entity.DTOs.Order.Select;

namespace Data.Interfaces.Implements.Producers
{
    public interface IProducerRepository : IDataGeneric<Producer>
    {
        Task<int?> GetIdProducer(int userId);
        Task<Producer?> GetByCodeProducer(string codeProducer);
        Task<ContactDto> GetContactProducer(int producerId);

    }
}
