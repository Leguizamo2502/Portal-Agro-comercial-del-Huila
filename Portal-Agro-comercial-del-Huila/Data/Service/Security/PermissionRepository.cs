using Data.Interfaces.Implements.Security;
using Data.Repository;
using Entity.Domain.Models.Implements.Security;
using Entity.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Service.Security
{
    public class PermissionRepository : DataGeneric<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
