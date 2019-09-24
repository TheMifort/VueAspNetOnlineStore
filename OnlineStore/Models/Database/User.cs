using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Models.Database
{
    public class User : IdentityUser
    {
        public Guid? Customer { get; set; }

        public virtual List<RefreshToken> RefreshTokens { get; set; }
    }
}
