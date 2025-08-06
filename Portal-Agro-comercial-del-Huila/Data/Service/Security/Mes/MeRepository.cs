using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements.Security.Mes;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Security;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;

namespace Data.Service.Security.Mes
{
    public class MeRepository : IMeRepository
    {
        private readonly ApplicationDbContext _context;
        public MeRepository(ApplicationDbContext context)
        {
            _context = context;   
        }

        public async Task<User?> GetUserWithPersonAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        }




        public async Task<IEnumerable<RolUser>> GetUserRolesWithPermissionsAsync(int userId)
        {
            return await _context.RolUsers
                                .Include(ur => ur.Rol)
                                    .ThenInclude(r => r.RolFormPermissions)
                                        .ThenInclude(rfp => rfp.Permission)
                                .Include(ur => ur.Rol)
                                    .ThenInclude(r => r.RolFormPermissions)
                                        .ThenInclude(rfp => rfp.Form)
                                .Where(ur => ur.UserId == userId && !ur.IsDeleted)
                                .ToListAsync();
        }


        public async Task<IEnumerable<Form>> GetFormsWithModulesByIdsAsync(List<int> formIds)
        {
            return await _context.Forms
                    .Include(f => f.FormModules)
                        .ThenInclude(mf => mf.Module)
                    .Where(f => formIds.Contains(f.Id) && !f.IsDeleted)
                    .ToListAsync();
        }

    }
}
