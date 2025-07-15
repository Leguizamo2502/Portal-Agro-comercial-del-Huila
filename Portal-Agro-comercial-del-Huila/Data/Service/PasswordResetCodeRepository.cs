using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.Implements;
using Data.Repository;
using Entity.Domain.Models.Implements.Auth;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Service
{
    public class PasswordResetCodeRepository : DataGeneric<PasswordResetCode>, IPasswordResetCodeRepository
    {
        public PasswordResetCodeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<PasswordResetCode?> GetValidCodeAsync(string email, string code)
        {
            return await _dbSet.FirstOrDefaultAsync(c =>
                c.Email == email &&
                c.Code == code &&
                !c.IsUsed &&
                c.Expiration > DateTime.UtcNow);
        }
    }

}
