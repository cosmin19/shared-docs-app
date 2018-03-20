using ASP.NET_Core_Basic_Web_API.Context;
using ASP.NET_Core_Basic_Web_API.Features.Account.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Basic_Web_API.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DbApplicationContext _context;

        public UserService(DbApplicationContext context)
        {
            this._context = context;
        }


        public virtual async Task<IList<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        public virtual async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            return user;
        }

        public virtual async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();

            return user;
        }

        public virtual async Task<User> GetUserByUserName(string userName)
        {
            var user = await _context.Users.Where(u => u.UserName.Equals(userName)).FirstOrDefaultAsync();

            return user;
        }

        public virtual async Task Insert(User user)
        {
            if(user == null)
            {
                return;
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
