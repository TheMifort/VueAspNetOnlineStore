using System.Security.Claims;

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
    }
}