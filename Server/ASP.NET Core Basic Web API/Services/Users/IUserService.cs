using ASP.NET_Core_Basic_Web_API.Features.Account.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Basic_Web_API.Services.Users
{
    public interface IUserService
    {
        Task<IList<User>> GetAllUsers();

        Task<User> GetUserById(int id);


        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByUserName(string userName);

        Task Insert(User user);
    }
}
