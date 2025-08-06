using Business.Interfaces.Implements.Security;
using Business.Repository;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Security.Create.NewFolder;
using Entity.DTOs.Security.Selects.Module;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Security
{
    public class ModuleService : BusinessGeneric<ModuleRegisterDto, ModuleSelectDto, Module>, IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        public ModuleService(IDataGeneric<Module> data, IMapper mapper, IModuleRepository moduleRepository) : base(data, mapper)
        {
            _moduleRepository = moduleRepository;
        }
    }
}
