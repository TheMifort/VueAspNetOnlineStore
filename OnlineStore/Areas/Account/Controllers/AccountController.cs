using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Areas.Account.Models.Request.Account;
using OnlineStore.Database;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Account.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
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

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] SignOutRequestModel request)
        {
            if (!string.IsNullOrEmpty(request?.RefreshToken))
            {
                var user = await _userManager.GetUserAsync(User);

                var refreshTokenEntry =
                    user.RefreshTokens.FirstOrDefault(e =>
                        e.Token == request.RefreshToken);

                if (refreshTokenEntry == null)
                {
                    ModelState.TryAddModelError("RefreshToken.Invalid", "RefreshToken неверен");
                    return BadRequest(ModelState);
                }

                user.RefreshTokens.Remove(refreshTokenEntry);

                await _databaseContext.SaveChangesAsync();

                return new OkResult();
            }

            ModelState.TryAddModelError("RefreshToken.Invalid", "RefreshToken неверен");
            return BadRequest(ModelState);
        }

        [HttpPost("signoutall")]
        public async Task<IActionResult> SignOutAll([FromBody] SignOutAllRequestModel request)
        {
            if (!string.IsNullOrEmpty(request?.RefreshToken))
            {
                var user = await _userManager.GetUserAsync(User);

                var refreshTokenEntry =
                    user.RefreshTokens.FirstOrDefault(e =>
                        e.Token == request.RefreshToken && e.ExpiresAt > DateTime.Now.ToUniversalTime());

                if (refreshTokenEntry == null)
                {
                    ModelState.TryAddModelError("RefreshToken.Invalid", "RefreshToken неверен");
                    return BadRequest(ModelState);
                }

                user.RefreshTokens.RemoveAll(e => request.SaveCurrentLogin == false || e.Id != refreshTokenEntry.Id);

                await _databaseContext.SaveChangesAsync();

                return new OkResult();
            }

            ModelState.TryAddModelError("RefreshToken.Invalid", "RefreshToken неверен");
            return BadRequest(ModelState);
        }
    }
}