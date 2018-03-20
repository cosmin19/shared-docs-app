using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ASP.NET_Core_Basic_Web_API.Infrastructure.Jwt.Entities;
using System.Linq;
using System.Collections.Generic;
using ASP.NET_Core_Basic_Web_API.Features.Account.Dto;
using ASP.NET_Core_Basic_Web_API.Services.Users;
using ASP.NET_Core_Basic_Web_API.Infrastructure.Utilities;
using ASP.NET_Core_Basic_Web_API.Features.Account.Entities;
using ASP.NET_Core_Basic_Web_API.Services.Jwt;
using ASP.NET_Core_Basic_Web_API.Models;

namespace ASP.NET_Core_Basic_Web_API.Features.Account
{
    [Route("api/Account")]
    public class AccountController : Controller
    {
        #region Fields
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public AccountController(
            IJwtService jwtService,
            ILoggerFactory loggerFactory,
            IUserService userService)
        {
            _logger = loggerFactory.CreateLogger<AccountController>();

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            this._jwtService = jwtService;
            this._userService = userService;

        }
        #endregion

        #region Methods
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto applicationUser)
        {
            var identity = await _jwtService.GetClaimsIdentity(applicationUser);
            if (identity == null)
            {
                _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                return BadRequest(new MessageDto() { Message = "Invalid credentials" });
            }

            // Serialize and return the response
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                access_token = _jwtService.GenerateEncodedToken(applicationUser.UserName, identity),
                expires_in = _jwtService.GetValidForTotalSeconds()
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    IsActivated = false,
                    LastName = model.LastName,
                    Password = PasswordUtils.HashPassword(model.Password),
                    Role = "User",
                    UserName = model.UserName,
                    CreatedOnUtc = DateTime.Now,
                    Valid = true
                };

                await _userService.Insert(newUser);
                return new OkObjectResult(new MessageDto() { Message="Registration successful."});
            }

            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList()
                           .Select(c => c.Select(x => x.ErrorMessage).FirstOrDefault());

            return BadRequest(errors);
        }
        #endregion
    }
}