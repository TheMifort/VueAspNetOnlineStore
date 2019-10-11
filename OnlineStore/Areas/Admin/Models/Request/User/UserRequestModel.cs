using System;
using System.Collections.Generic;

namespace OnlineStore.Areas.Admin.Models.Request.User
{
    public class UserRequestModel
    {
        public string Id { get; set; }
        public string Password { get; set; }

        public List<string> Roles { get; set; }
        public Guid? CustomerId { get; set; }
    }
}