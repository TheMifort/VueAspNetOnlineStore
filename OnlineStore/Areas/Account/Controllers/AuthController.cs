using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineStore.Abstract;
using OnlineStore.Areas.Account.Models.Request.Auth;
using OnlineStore.Areas.Account.Models.Response.Auth;
using OnlineStore.Database;
using OnlineStore.Helpers;
using OnlineStore.Models;
using OnlineStore.Models.Database;
using OnlineStore.Workers;

namespace OnlineStore.Areas.Account.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Account")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DatabaseContext _databaseContext;
        private readonly IIdentityWorker _identityWorker;

        public AuthController(UserManager<User> userManager, DatabaseContext databaseContext, AuthOptions authOptions)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
            _identityWorker = new IdentityWorker(userManager, databaseContext, authOptions); //TODO Add to DI
        }

        [HttpPost("Token")]
        public async Task<IActionResult> Token([FromBody] AuthUserRefreshTokenRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrEmpty(model.RefreshToken))
            {
                var refreshTokenEntry = await
                    _databaseContext.RefreshTokens.FirstOrDefaultAsync(e => e.Token == model.RefreshToken);

                if (refreshTokenEntry.ExpiresAt < DateTime.Now.ToUniversalTime())
                {
                    ModelState.TryAddModelError("RefreshToken.Expired", "RefreshToken прострочен");
                    return BadRequest(ModelState);
                }

                var identity = await _identityWorker.GetClaimsIdentity(model.RefreshToken);
                if (identity != null)
                {
                    var refreshToken = _identityWorker.GenerateRefreshToken();
                    var expiresAt = DateTime.Now.ToUniversalTime().Add(new TimeSpan(days: 30, 0, 0, 0));
                    var role = User.GetUserRole();
                    var response = new AuthUserResponseModel
                    {
                        AccessToken = _identityWorker.GenerateJwtToken(identity),
                        RefreshToken = refreshToken,
                        Name = identity.Name,
                        ExpiresAt = expiresAt,
                        Role = role
                    };

                    refreshTokenEntry.Token = refreshToken;
                    refreshTokenEntry.ExpiresAt = expiresAt;
                    await _databaseContext.SaveChangesAsync();

                    var json = JsonConvert.SerializeObject(response,
                        new JsonSerializerSettings { Formatting = Formatting.Indented });
                    return new OkObjectResult(json);
                }
            }

            ModelState.TryAddModelError("RefreshToken.Invalid", "RefreshToken неверен");
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthUserPasswordRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                var identity = await _identityWorker.GetClaimsIdentity(model.UserName, model.Password);
                if (identity != null)
                {
                    var refreshToken = _identityWorker.GenerateRefreshToken();
                    var expiresAt = DateTime.Now.ToUniversalTime().Add(new TimeSpan(days: 30, 0, 0, 0));
                    var role = User.IsInRole("Admin") ? "Admin" : (User.IsInRole("Manager") ? "Manager" : "User");
                    var response = new AuthUserResponseModel
                    {
                        AccessToken = _identityWorker.GenerateJwtToken(identity),
                        RefreshToken = refreshToken,
                        Name = identity.Name,
                        ExpiresAt = expiresAt,
                        Role = role
                    };
                    var user = await _userManager.GetUserAsync(User);
                    user.RefreshTokens.Add(new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiresAt = expiresAt
                    });
                    await _databaseContext.SaveChangesAsync();

                    var json = JsonConvert.SerializeObject(response,
                        new JsonSerializerSettings { Formatting = Formatting.Indented });
                    return new OkObjectResult(json);
                }
            }

            ModelState.TryAddModelError("Password.Invalid", "Логин и/или пароль неверны");
            return BadRequest(ModelState);
        }
    }
}
