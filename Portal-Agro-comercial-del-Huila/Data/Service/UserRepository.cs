using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements;
using Data.Repository;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service
{
    public class UserRepository : DataGeneric<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Person)
                .Include(u => u.RolUsers)
                    .ThenInclude(ru => ru.Rol)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == false);
        }

        public async Task<User> LoginUser(LoginUserDto loginDto)
        {
            bool suceeded = false;

            var user = await _dbSet
                .FirstOrDefaultAsync(u =>
                            u.Email == loginDto.Email &&
                            u.Password == (loginDto.Password));

            suceeded = (user != null) ? true : throw new UnauthorizedAccessException("Credenciales inválidas");

            return user;
        }



        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && u.IsDeleted == false);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Person)
                .Include(u => u.RolUsers).ThenInclude(ru => ru.Rol)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }


        


    }
 
}
