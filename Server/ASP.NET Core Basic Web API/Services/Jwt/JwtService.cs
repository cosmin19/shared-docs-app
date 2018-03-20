using ASP.NET_Core_Basic_Web_API.Features.Account.Dto;
using ASP.NET_Core_Basic_Web_API.Infrastructure.Jwt.Entities;
using ASP.NET_Core_Basic_Web_API.Infrastructure.Utilities;
using ASP.NET_Core_Basic_Web_API.Services.Users;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ASP.NET_Core_Basic_Web_API.Services.Jwt
{
    public class JwtService : IJwtService
    {
        #region Fields
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public JwtService(IOptions<JwtIssuerOptions> jwtOptions, IUserService userService)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);

            this._userService = userService;
        }

        #endregion

        #region Methods
        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        public async Task<ClaimsIdentity> GetClaimsIdentity(LoginDto user)
        {
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
                {
                    var userEntity = await _userService.GetUserByUserName(user.UserName);
                    if (userEntity != null)
                    {
                        if (PasswordUtils.VerifyHashedPassword(userEntity.Password, user.Password))
                        {
                            var roles = new List<string>() { userEntity.Role };
                            IList<string> claims = new List<string>();

                            return await Task.FromResult(GenerateClaimsIdentity(userEntity.UserName, userEntity.Id, roles, claims));
                        }
                    }
                }
            }
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, int id, IList<string> roles, IList<string> claims)
        {

            var claimsIdentity = new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(JwtStaticConstants.Strings.JwtClaimIdentifiers.Id, id.ToString()),
                new Claim(JwtStaticConstants.Strings.JwtClaimIdentifiers.Rol, JwtStaticConstants.Strings.JwtClaims.ApiAccess),
            });

            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(JwtStaticConstants.Strings.JwtClaimIdentifiers.Roles, role));
            }

            foreach (var claim in claims)
            {
                claimsIdentity.AddClaim(new Claim(JwtStaticConstants.Strings.JwtClaimIdentifiers.Permission, claim));
            }

            return claimsIdentity;
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            List<Claim> claimsList = new List<Claim>();
            claimsList.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
            claimsList.Add(new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()));
            claimsList.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64));
            claimsList.AddRange(identity.Claims);

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claimsList,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public int GetValidForTotalSeconds()
        {
            return (int)_jwtOptions.ValidFor.TotalSeconds;
        }
        #endregion
    }
}
