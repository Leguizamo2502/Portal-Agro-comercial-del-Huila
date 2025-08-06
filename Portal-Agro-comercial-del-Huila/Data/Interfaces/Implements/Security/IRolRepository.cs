using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Security;

namespace Data.Interfaces.Implements.Security
{
    public interface IRolRepository : IDataGeneric<Rol>
    {
    }
}
