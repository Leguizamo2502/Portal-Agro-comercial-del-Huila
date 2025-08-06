using Business.Interfaces.Implements.Security;
using Business.Repository;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.Rols;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Services.Security
{
    public class FormService : BusinessGeneric<FormRegisterDto, FormSelectDto, Form>, IFormService
    {
        private readonly IFormRepository _formRepository;
        public FormService(IDataGeneric<Form> data, IMapper mapper, IFormRepository formRepository) : base(data, mapper)
        {
            _formRepository = formRepository;
        }
    }
}
