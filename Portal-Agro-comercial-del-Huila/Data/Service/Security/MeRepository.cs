using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Security;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;

namespace Data.Service.Security
{
    public class MeRepository : IMeRepository
    {
        private readonly ApplicationDbContext _context;
        public MeRepository(ApplicationDbContext context)
        {
            _context = context;   
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Person> GetPersonByUserAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<List<Rol>> GetRolesByUserAsync(int userId)
        {
            return await _context.RolUsers
               .Where(ru => ru.UserId == userId && !ru.IsDeleted)
               .Select(ru => ru.Rol)
               .Include(r => r.RolFormPermissions.Where(rfp => !rfp.IsDeleted))
                   .ThenInclude(rfp => rfp.Permission)
               .Include(r => r.RolFormPermissions)
                   .ThenInclude(rfp => rfp.Form)
                       .ThenInclude(f => f.FormModules)
                           .ThenInclude(fm => fm.Module)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<List<RolFormPermission>> GetPermissionsByUserAsync(int userId)
        {
            return await _context.RolUsers
                .Where(ru => ru.UserId == userId && !ru.IsDeleted)
                .SelectMany(ru => ru.Rol.RolFormPermissions)
                .Where(rfp => !rfp.IsDeleted)
                .Include(rfp => rfp.Permission)
                .Include(rfp => rfp.Form)
                    .ThenInclude(f => f.FormModules)
                        .ThenInclude(fm => fm.Module)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
