using Enviroself.Context;
using Enviroself.Features.Account.Entities;
using Enviroself.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Services.Users
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

        public virtual async Task<MessageDto> Insert(User user)
        {
            if (user == null)
            {
                return new MessageDto() { Success = false, Message = "Error"};
            }
            if(_context.Users.Where(u => u.UserName.ToLower().Equals(user.UserName)).FirstOrDefault() != null)
            {
                return new MessageDto() { Success = false, Message = "Username already taken." };
            }
            if (_context.Users.Where(u => u.Email.ToLower().Equals(user.Email)).FirstOrDefault() != null)
            {
                return new MessageDto() { Success = false, Message = "Email already exists." };

            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new MessageDto() { Success = true, Message = "Success" };
        }
    }
}
