using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Admin.Models.Request.User
{
    public class CreateUserRequestModel
    {
        [Required] public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}