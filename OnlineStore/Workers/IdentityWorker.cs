using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Abstract;
using OnlineStore.Database;
using OnlineStore.Helpers;
using OnlineStore.Models;
using OnlineStore.Models.Database;

namespace OnlineStore.Workers
{
    public class IdentityWorker : IIdentityWorker
    {
        private readonly UserManager<User> _userManager;
        private readonly DatabaseContext _databaseContext;
        private readonly AuthOptions _authOptions;

        public IdentityWorker(UserManager<User> userManager, DatabaseContext databaseContext,
            AuthOptions authOptions)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
            _authOptions = authOptions;
        }

        public async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);
            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);
            // check the credentials  

            if (await _userManager.CheckPasswordAsync(userToVerify, password))
                return await GetClaimsIdentity(userToVerify);

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        public async Task<ClaimsIdentity> GetClaimsIdentity(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return await Task.FromResult<ClaimsIdentity>(null);
            // get the user to verifty

            var refreshTokenEntry =
                await _databaseContext.RefreshTokens.FirstOrDefaultAsync(e => e.Token == refreshToken);

            if (refreshTokenEntry.ExpiresAt.HasValue && refreshTokenEntry.ExpiresAt <= DateTime.Now)
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = refreshTokenEntry.User;

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);
            // check the credentials  

            return await GetClaimsIdentity(userToVerify);
        }

        public string GenerateJwtToken(ClaimsIdentity claims)
        {
            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                notBefore: now,
                claims: claims.Claims,
                expires: now.Add(TimeSpan.FromMinutes(_authOptions.Lifetime)),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roles.GetRole())
            };

            return await Task.FromResult(new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType));
        }
    }
}