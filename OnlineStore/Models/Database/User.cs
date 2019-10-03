using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Models.Database
{
    public class User : IdentityUser
    {
        public virtual Customer Customer { get; set; }
    }
}
