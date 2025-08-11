using Data.Interfaces.Implements.Location;
using Data.Interfaces.Implements.Security;
using Data.Repository;
using Entity.Domain.Models.Implements.Location;
using Entity.Domain.Models.Implements.Security;
using Entity.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Service.Location
{
    public class DepartmentRepository : DataGeneric<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
