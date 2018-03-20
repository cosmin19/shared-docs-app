using ASP.NET_Core_Basic_Web_API.Features.Account.Dto;
using ASP.NET_Core_Basic_Web_API.Infrastructure.Jwt.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP.NET_Core_Basic_Web_API.Services.Jwt
{
    public interface IJwtService
    {
        Task<ClaimsIdentity> GetClaimsIdentity(LoginDto user);

        ClaimsIdentity GenerateClaimsIdentity(string userName, int id, IList<string> roles, IList<string> claims);

        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);

        int GetValidForTotalSeconds();
    }
}
