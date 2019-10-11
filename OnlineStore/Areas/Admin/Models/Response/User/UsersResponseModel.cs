using System.Collections.Generic;
using OnlineStore.Areas.Admin.Models.Response.Customer;

namespace OnlineStore.Areas.Admin.Models.Response.User
{
    public class UsersResponseModel
    {
        public IEnumerable<UserResponseModel> Users { get; set; }
        public IEnumerable<string> AllRoles { get; set; }
        public IEnumerable<CustomerResponseModel> AllCustomers { get; set; }
    }
}