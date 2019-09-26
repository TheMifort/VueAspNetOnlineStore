using System.Collections.Generic;
using OnlineStore.Areas.Admin.Models.Request.Customer;

namespace OnlineStore.Areas.Admin.Models.Request.User
{
    public class UserRequestModel
    {
        public string Id { get; set; }s

        public List<string> Roles { get; set; }
        public CustomerRequestModel Customer { get; set; }
    }
}