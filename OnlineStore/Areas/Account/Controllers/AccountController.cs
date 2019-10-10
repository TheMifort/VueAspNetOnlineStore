using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Areas.Account.Models.Request.Account;
using OnlineStore.Database;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Account.Controllers
{
    [Authorize]
    [Route("api/[area]")]
    [Area("Account")]
    public class AccountController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserManager<User> _userManager;

        public AccountController(DatabaseContext databaseContext, UserManager<User> userManager)
        {
            _databaseContext = databaseContext;
            _userManager = userManager;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignOut()
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(e => e.UserName == User.Identity.Name);
            var claim = User.FindFirst("App.Identity.Id");
            var refreshTokenEntry =
                user.RefreshTokens.FirstOrDefault(e =>
                    e.Token == claim.Value && e.ExpiresAt > DateTime.Now.ToUniversalTime());

            if (refreshTokenEntry == null)
            {
                ModelState.TryAddModelError("RefreshToken.Invalid", "RefreshToken неверен");
                return BadRequest(ModelState);
            }

            user.RefreshTokens.Remove(refreshTokenEntry);

            await _databaseContext.SaveChangesAsync();

            return new OkResult();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignOutAll([FromBody] SignOutAllRequestModel request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(e => e.UserName == User.Identity.Name);

            var claim = User.FindFirst("App.Identity.Id");

            var refreshTokenEntry =
                user.RefreshTokens.FirstOrDefault(e =>
                    e.Token == claim.Value && e.ExpiresAt > DateTime.Now.ToUniversalTime());

            if (refreshTokenEntry == null)
            {
                ModelState.TryAddModelError("RefreshToken.Invalid", "RefreshToken неверен");
                return BadRequest(ModelState);
            }

            user.RefreshTokens.RemoveAll(e => request.SaveCurrentLogin == false || e.Id != refreshTokenEntry.Id);

            await _databaseContext.SaveChangesAsync();

            return new OkResult();

        }
    }
}