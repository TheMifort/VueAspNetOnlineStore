using System.Collections.Generic;

namespace OnlineStore.Areas.Admin.Models.Response.Role
{
    public class UserResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}