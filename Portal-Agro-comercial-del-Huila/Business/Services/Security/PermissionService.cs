using Business.Interfaces.Implements.Security;
using Business.Repository;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Security.Create.NewFolder;
using Entity.DTOs.Security.Create.Permissions;
using Entity.DTOs.Security.Selects.Module;
using Entity.DTOs.Security.Selects.Permissions;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Security
{
    public class PermissionService : BusinessGeneric<PermissionRegisterDto, PermissionSelectDto, Permission>, IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionService(IDataGeneric<Permission> data, IMapper mapper, IPermissionRepository permissionRepository) : base(data, mapper)
        {
            _permissionRepository = permissionRepository;
        }
    }
}
