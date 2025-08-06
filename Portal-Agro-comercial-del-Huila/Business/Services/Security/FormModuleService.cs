using Business.Interfaces.Implements.Security;
using Business.Repository;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Security.Create.FormModule;
using Entity.DTOs.Security.Create.Rols;
using Entity.DTOs.Security.Selects.FormModule;
using Entity.DTOs.Security.Selects.Rols;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business.Services.Security
{
    public class FormModuleService : BusinessGeneric<FormModuleRegisterDto, FormModuleSelectDto, FormModule>, IFormModuleService
    {
        private readonly IFormModuleRepository _formModuleRepository;
        public FormModuleService(IDataGeneric<FormModule> data, IMapper mapper, IFormModuleRepository formModuleRepository) : base(data, mapper)
        {
            _formModuleRepository = formModuleRepository;
        }
        public override async Task<IEnumerable<FormModuleSelectDto>> GetAllAsync()
        {
            try
            {
                var entities = await _formModuleRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<FormModuleSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }

        }
    }
}