using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Account.Models.Request.Auth
{
    public class AuthUserRefreshTokenRequestModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}