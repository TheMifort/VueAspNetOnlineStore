using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Account.Models.Request.Account
{
    public class SignOutRequestModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}