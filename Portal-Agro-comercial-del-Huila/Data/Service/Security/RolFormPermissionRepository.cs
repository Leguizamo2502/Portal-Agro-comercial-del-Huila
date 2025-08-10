using Data.Interfaces.Implements.Security;
using Data.Repository;
using Entity.Domain.Models.Implements.Security;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Service.Security
{
    public class RolFormPermissionRepository : DataGeneric<RolFormPermission>, IRolFormPermissionRepository
    {
        public RolFormPermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<RolFormPermission>> GetAllAsync()
        {
            return await _dbSet
                .Include(rfp => rfp.Rol)
                .Include(rfp => rfp.Form)
                .Include(rfp => rfp.Permission)
                .Where(rfp => rfp.IsDeleted == false)
                .ToListAsync();
        }

    }


}