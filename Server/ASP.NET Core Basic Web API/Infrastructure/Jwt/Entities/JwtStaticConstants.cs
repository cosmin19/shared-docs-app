using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Basic_Web_API.Infrastructure.Jwt.Entities
{
    public static class JwtStaticConstants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id", Roles = "roles", Permission = "permission";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
                public const string ApiAccessExtended = "api_access_extended";
            }
        }

    }
}
