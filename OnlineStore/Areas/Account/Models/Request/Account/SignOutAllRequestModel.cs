using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Account.Models.Request.Account
{
    public class SignOutAllRequestModel
    {
        [Required] public string RefreshToken { get; set; }
        public bool? SaveCurrentLogin { get; set; }
    }
}