using Enviroself.Context;
using Enviroself.Features.Account.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Enviroself.Services.Auth
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DbApplicationContext _appDbContext;

        public IdentityService(IHttpContextAccessor httpContextAccessor, DbApplicationContext appDbContext)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._appDbContext = appDbContext;
        }

        public virtual async Task<User> GetCurrentPersonIdentityAsync()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var currentUser = await _appDbContext.Users.Where(x => x.UserName.ToLower().Equals(userName.ToLower()) || 
                                                              x.Email.ToLower().Equals(userName.ToLower()))
                                                              .FirstOrDefaultAsync();

            return currentUser;
        }
    }
}
