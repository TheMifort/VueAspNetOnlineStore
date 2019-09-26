using System.Collections.Generic;

namespace OnlineStore.Areas.Admin.Models.Response.Role
{
    public class RoleUserResponseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}