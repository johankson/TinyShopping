using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TinyShopping.Api.Data.Models;
using TinyShopping.Api.Services;

namespace TinyShopping.Api.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;
        readonly TokenService token;

        public UserController(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager, TokenService token)
        {
            this.token = token;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet("adduser/{user}")]
        public async Task<object> AddUser(string user, [FromQuery]string password)
        {
            var r = await userManager.CreateAsync(new ApplicationUser()
            {
                UserName = user
            }, password);
            return r;
        }

        [HttpGet("login/{user}")]
        public async Task<string> Login(string user, [FromQuery]string password)
        {
            var r = await signInManager.PasswordSignInAsync(user, password, true, false);
            if (r.Succeeded)
                return token.GenerateToken(user);
            Response.StatusCode = 401;
            return string.Empty;
        }



    }
}
