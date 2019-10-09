using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Areas.Account.Models.Request.Account;
using OnlineStore.Areas.Account.Models.Response.Account;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Account.Controllers
{
    [Authorize]
    [Route("api/[area]")]
    [Area("Account")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
                if (result.Succeeded)
                {
                    var user = await _signInManager.UserManager.Users.FirstOrDefaultAsync(e => e.UserName == model.UserName);
                    var roles = await _signInManager.UserManager.GetRolesAsync(user);
                    var response = new LoginResponseModel();

                    if (roles.Contains("Admin"))
                        response.Role = "Admin";
                    else if (roles.Contains("Manager"))
                        response.Role = "Manager";
                    else if (roles.Contains("User"))
                        response.Role = "User";


                    return Ok(response);
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }

            return BadRequest();
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}