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
        Task<int> SalesNumberByCode(string codeProducer);
        Task<string?> GetCodeProducer(int producerId);
        Task<Producer?> GetByIdWithSocialLinksAsync(int id);
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;
        Task<double> GetAverageRatingAsync(int producerId);

    }
}
