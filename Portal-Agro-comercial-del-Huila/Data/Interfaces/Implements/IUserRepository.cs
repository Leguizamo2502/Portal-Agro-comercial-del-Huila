using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;

namespace Data.Interfaces.Implements
{
    public interface IUserRepository : IDataGeneric<User>
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task<User> LoginUser(LoginUserDto loginDto);
        


    }
}
