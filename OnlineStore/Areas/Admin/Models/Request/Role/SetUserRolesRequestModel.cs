using System.Collections.Generic;

namespace OnlineStore.Areas.Admin.Models.Request.Role
{
    public class SetUserRolesRequestModel
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}