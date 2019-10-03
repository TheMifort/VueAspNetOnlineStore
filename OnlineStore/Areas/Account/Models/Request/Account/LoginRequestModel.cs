using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Account.Models.Request.Account
{
    public class LoginRequestModel
    {
        [Required] public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}