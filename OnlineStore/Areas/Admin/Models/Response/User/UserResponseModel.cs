using System.Collections.Generic;
using OnlineStore.Areas.Admin.Models.Response.Customer;

namespace OnlineStore.Areas.Admin.Models.Response.User
{
    public class UserResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public List<string> Roles { get; set; }
        public CustomerResponseModel Customer { get; set; }
    }
}