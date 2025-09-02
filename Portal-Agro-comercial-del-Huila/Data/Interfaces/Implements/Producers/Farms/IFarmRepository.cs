using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Producers;

namespace Data.Interfaces.Implements.Producers.Farms
{
    public interface IFarmRepository : IDataGeneric<Farm>
    {
        Task<IEnumerable<Farm>> GetByProducer(int? producerId);
        Task<IEnumerable<Farm>> GetByProducerCode(string producerCode);

    }
}
