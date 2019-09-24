using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models.Database
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public virtual User User { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
