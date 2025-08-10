using Business.Interfaces.Implements.Security;
using Business.Repository;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Security;
using Entity.DTOs.Security.Create.FormModule;
using Entity.DTOs.Security.Create.RolFormPermission;
using Entity.DTOs.Security.Selects.FormModule;
using Entity.DTOs.Security.Selects.RolFormPermission;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business.Services.Security
{
    public class RolFormPermissionService : BusinessGeneric<RolFormPermissionRegisterDto, RolFormPermissionSelectDto, RolFormPermission>, IRolFormPermissionService
    {
        private readonly IRolFormPermissionRepository _rolFormPermissionRepository;
        public RolFormPermissionService(IDataGeneric<RolFormPermission> data, IMapper mapper, IRolFormPermissionRepository rolFormPermissionRepository) : base(data, mapper)
        {
            _rolFormPermissionRepository = rolFormPermissionRepository;
        }
        public override async Task<IEnumerable<RolFormPermissionSelectDto>> GetAllAsync()
        {
            try
            {
                var entities = await _rolFormPermissionRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<RolFormPermissionSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }

        }
    }
}