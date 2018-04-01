using Enviroself.Features.Account.Dto;
using Enviroself.Infrastructure.Jwt.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Enviroself.Services.Jwt
{
    public interface IJwtService
    {
        Task<ClaimsIdentity> GetClaimsIdentity(LoginDto user);

        ClaimsIdentity GenerateClaimsIdentity(string userName, int id, IList<string> roles, IList<string> claims);

        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);

        int GetValidForTotalSeconds();
    }
}
