using System;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Models.Database
{
    public class User : IdentityUser
    {
        public Guid? Customer { get; set; }
    }
}
