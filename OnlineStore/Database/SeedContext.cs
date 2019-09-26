using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OnlineStore.Models.Database;

namespace OnlineStore.Database
{
    public class SeedContext
    {
        public async Task Seed(DatabaseContext context)
        {
            var roles = new List<string>
            {
                "Admin",
                "Manager",
                "User"
            };

            var user = new User
            {
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<IdentityRole>(context);

            foreach (var roleName in roles)
            {
                if (!roleStore.Roles.Any(e => e.Name == roleName))
                {
                    await roleStore.CreateAsync(new IdentityRole(roleName) {NormalizedName = roleName.ToUpper()});
                }
            }

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "+");
                user.PasswordHash = hashed;
                using var userStore = new UserStore<User>(context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "Admin");
            }

            await context.SaveChangesAsync();
        }
    }
}