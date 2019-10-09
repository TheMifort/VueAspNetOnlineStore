using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Account.Models.Request.Auth
{
    public class AuthUserPasswordRequestModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}