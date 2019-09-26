using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [Area("Account")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.TryAddModelError("Model.Invalid", "Модель не является валидной");
        //        return BadRequest(ModelState);
        //    }

        //    var identityUser = new User
        //    {
        //        UserName = model.UserName,
        //    };
        //    var result = await _userManager.CreateAsync(identityUser, model.Password);

        //    if (!result.Succeeded)
        //    {
        //        foreach (var e in result.Errors)
        //        {
        //            ModelState.TryAddModelError(e.Code, e.Description);
        //        }

        //        return new BadRequestObjectResult(ModelState);
        //    }

        //    return new OkResult();
        //}
    }
}