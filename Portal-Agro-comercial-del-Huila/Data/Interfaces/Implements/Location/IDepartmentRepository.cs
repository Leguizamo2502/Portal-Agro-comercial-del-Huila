using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Location;
using Entity.Domain.Models.Implements.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.Implements.Location
{
    public interface IDepartmentRepository : IDataGeneric<Department>
    {
    }
}
