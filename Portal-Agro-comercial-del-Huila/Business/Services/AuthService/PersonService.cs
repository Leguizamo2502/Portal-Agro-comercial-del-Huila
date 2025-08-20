using Business.Interfaces.Implements.Auth;
using Business.Repository;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;
using MapsterMapper;
using Utilities.Exceptions;
using Utilities.Helpers.Business;

namespace Business.Services.AuthService
{
    public class PersonService : BusinessGeneric<PersonRegisterDto, PersonSelectDto, Person>, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        public PersonService(IDataGeneric<Person> data, IMapper mapper, IPersonRepository personRepository) : base(data, mapper)
        {
            _personRepository = personRepository;
        }

    }
}
