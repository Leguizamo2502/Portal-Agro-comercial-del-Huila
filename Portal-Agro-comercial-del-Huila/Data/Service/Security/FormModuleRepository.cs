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
    public class FormModuleRepository : DataGeneric<FormModule>, IFormModuleRepository
    {
        public FormModuleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<FormModule>> GetAllAsync()
        {
            return await _dbSet
                .Include(fm => fm.Form)
                .Include(fm => fm.Module)
                .Where(fm => fm.IsDeleted == false)
                .ToListAsync();
        }

    }


}
