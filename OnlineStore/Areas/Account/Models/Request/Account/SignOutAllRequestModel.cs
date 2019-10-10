using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Account.Models.Request.Account
{
    public class SignOutAllRequestModel
    {
        public bool? SaveCurrentLogin { get; set; }
    }
}