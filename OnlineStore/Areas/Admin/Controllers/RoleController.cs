using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Areas.Admin.Models.Request.Role;
using OnlineStore.Areas.Admin.Models.Response.Role;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetUsers()
        {
            var result = new List<RoleUserResponseModel>();

            foreach (var user in _userManager.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new RoleUserResponseModel
                {
                    Id = user.Id,
                    UserName =  user.UserName,
                    Roles = roles.ToList()
                });
            }

            return Ok(result);
        }

        public async Task<IActionResult> GetUserRoles(GetUserRolesRequestModel model)
        {
            // получаем пользователя
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (User.IsInRole("Admin") && (await _userManager.GetRolesAsync(user)).Contains("RootAdmin"))
                return Forbid();

            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var requestModel = new ChangeRoleResponseModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserRoles = userRoles,
                    AllRoles = allRoles.Select(e=>e.Name).ToList()
                };
                return Ok(requestModel);
            }

            return NotFound();
        }

        public async Task<IActionResult> SetUserRoles(SetUserRolesRequestModel model)
        {
            // получаем пользователя
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (User.IsInRole("Admin") && (await _userManager.GetRolesAsync(user)).Contains("RootAdmin"))
                return Forbid();

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = model.Roles.Except(userRoles).ToList();
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(model.Roles).ToList();

                if (User.IsInRole("Admin") && (addedRoles.Contains("RootAdmin") || removedRoles.Contains("RootAdmin")))
                    return Forbid();

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return Ok();
            }

            return NotFound();
        }
    }
}