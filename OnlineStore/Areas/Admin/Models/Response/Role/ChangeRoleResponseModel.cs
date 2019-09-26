using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Areas.Admin.Models.Response.Role
{
    public class ChangeRoleResponseModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> AllRoles { get; set; } = new List<string>();
        public IList<string> UserRoles { get; set; } = new List<string>();
    }
}