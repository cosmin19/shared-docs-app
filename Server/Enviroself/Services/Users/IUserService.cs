using Enviroself.Features.Account.Entities;
using Enviroself.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Services.Users
{
    public interface IUserService
    {
        Task<IList<User>> GetAllUsers();

        Task<User> GetUserById(int id);


        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByUserName(string userName);

        Task<MessageDto> Insert(User user);
    }
}
