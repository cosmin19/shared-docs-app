using ASP.NET_Core_Basic_Web_API.Features.Account.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Basic_Web_API.Services.Auth
{
    public interface IIdentityService
    {
        Task<User> GetCurrentPersonIdentityAsync();
    }
}
