using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;

namespace Data.Interfaces.Implements.Auth
{
    public interface IPasswordResetCodeRepository : IDataGeneric<PasswordResetCode>
    {
        Task<PasswordResetCode?> GetValidCodeAsync(string email, string code);
    }
}
