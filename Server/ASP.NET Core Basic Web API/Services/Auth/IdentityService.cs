using ASP.NET_Core_Basic_Web_API.Context;
using ASP.NET_Core_Basic_Web_API.Features.Account.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP.NET_Core_Basic_Web_API.Services.Auth
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

            var currentUser = await _appDbContext.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();

            return currentUser;
        }
    }
}
