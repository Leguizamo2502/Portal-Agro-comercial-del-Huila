using Data.Interfaces.IRepository;
using Entity.Domain.Models.Implements.Auth;
using Entity.DTOs.Auth;
using Entity.DTOs.Order.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.Implements.Auth
{
    public interface IUserRepository : IDataGeneric<User>
    {
        Task<User?> GetDataBasic(int userId);
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task<User> LoginUser(LoginUserDto loginDto);
        Task<ContactDto> GetContactUser(int userId);
        Task<bool> ExistsByDocumentAsync(string identification);


    }
}
