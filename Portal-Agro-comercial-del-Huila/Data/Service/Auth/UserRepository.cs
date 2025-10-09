using Data.Interfaces.Implements.Auth;
using Data.Repository;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;
using Entity.DTOs.Order.Select;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service.Auth
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
                            u.Password == loginDto.Password);

            suceeded = user != null ? true : throw new UnauthorizedAccessException("Credenciales inválidas");

            return user;
        }



        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && u.IsDeleted == false);
        }

        public async Task<bool> ExistsByDocumentAsync(string identification)
        {
            return await _dbSet.AnyAsync(u=>u.Person.Identification == identification);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Person)
                .Include(u => u.RolUsers).ThenInclude(ru => ru.Rol)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }

        public async Task<User?> GetDataBasic(int userId)
        {
            return await _dbSet
                .Include(u => u.Person)
                    .ThenInclude(p => p.City)
                        .ThenInclude(c => c.Department)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet
                .Include(u => u.Person)
                .ThenInclude(p => p.City)
                .ToListAsync();
        }

        public async Task<ContactDto> GetContactUser(int userId)
        {
            var u = await _dbSet
                .AsNoTracking()
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
            if (u is null)
                throw new InvalidOperationException($"No se encontró el usuario con ID {userId}.");
            return new ContactDto
            {
                FirstName = u.Person.FirstName,
                Email = u.Email,
                LastName = u.Person.LastName,
            };
        }

    }
 
}
