using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;

namespace Data.Interfaces.Implements.Auth
{
    public interface IPersonRepository : IDataGeneric<Person>
    {
        Task<Person?> GetDataBasic(int personId);
    }
}
