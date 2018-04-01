using Enviroself.Services;
using Enviroself.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enviroself.Areas.Admin.Features.User
{
    [Area("Admin")]
    [Route("api/Admin/User")]
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _userService.GetAllUsers();

            return new ObjectResult(users);
        }

        // GET api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);

            return new ObjectResult(user);
        }
        #endregion

    }
}
