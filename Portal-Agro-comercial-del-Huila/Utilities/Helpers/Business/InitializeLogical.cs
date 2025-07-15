using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Utilities.Helpers.Business
{
    public static class InitializeLogical
    {
        public static void InitializeLogicalState(this object entity)
        {
            if (entity is BaseModel softDeletable)
            {
                softDeletable.IsDeleted = false;
            }
        }
    }
}
