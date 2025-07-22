using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Security
{
    public class Permission : BaseSecurity
    {
        public ICollection<RolFormPermission> RolFormPermissions { get; set; } = [];
    }
}
