using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Linq;
using Enviroself.Features.Account.Dto;
using Enviroself.Services.Users;
using Enviroself.Infrastructure.Utilities;
using Enviroself.Features.Account.Entities;
using Enviroself.Services.Jwt;
using Enviroself.Models;
using System.Net.Http;

namespace Enviroself.Features.Account
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
                return BadRequest(new MessageDto() { Success = false, Message = "Invalid credentials." });
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

                MessageDto result = await _userService.Insert(newUser);
                if (result.Success)
                    return new OkObjectResult(result);
                return BadRequest(result);
            }

            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList()
                           .Select(c => c.Select(x => x.ErrorMessage).FirstOrDefault());

            return BadRequest(errors);
        }

        [HttpPost("validateRecaptcha")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateRecaptcha([FromBody] CaptchaDto model)
        {
            string secretKey = "6LcIXk0UAAAAAPuQk42Nx6cqzGHz-QHe5kj1u_tB";
            var client = new HttpClient();
            string result = await client.GetStringAsync(
                string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                    secretKey,
                    model.Response)
                    );

            var captchaResponse = JsonConvert.DeserializeObject<RecapthcaResonseDto>(result);

            if (captchaResponse.Success)
                return new OkObjectResult(new MessageDto() { Success = true, Message = "Token Valid" });
            return BadRequest(new MessageDto() { Success = false, Message = "Token Invalid" });
        }
        #endregion

        #region Utilities

        #endregion
    }

    public class CaptchaDto {
        public string Response { get; set; }
    }

    public class RecapthcaResonseDto
    {
        public bool Success { get; set; }
        public DateTime Challenge_ts { get; set; }
        public string Hostname { get; set; }
    }

}