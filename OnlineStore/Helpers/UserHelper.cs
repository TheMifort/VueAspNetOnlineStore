using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using OnlineStore.Models.Database;

namespace OnlineStore.Helpers
{
    public static class UserHelper
    {
        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user.IsInRole("Admin") ? "Admin"
                : (user.IsInRole("Manager") ? "Manager"
                    : (user.IsInRole("User") ? "User" : ""));
        }

        public static string GetRole(this IList<string> roles)
        {
            return roles.Contains("Admin") ? "Admin"
                : (roles.Contains("Manager") ? "Manager"
                    : (roles.Contains("User") ? "User" : ""));
        }
    }
}