using System;

namespace OnlineStore.Areas.Account.Models.Response.Auth
{
    public class AuthUserResponseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Name { get; set; }
        public DateTime ExpiresAt { get; set; }

        public string Role { get; set; }
    }
}