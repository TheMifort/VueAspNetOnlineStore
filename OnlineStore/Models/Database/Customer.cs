using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models.Database
{
    public class Customer
    {
        [Required]
        [Column("ID")]
        public Guid Id { get; set; }

        [Required]
        [Column("NAME")]
        public string Name { get; set; }

        [Required]
        [Column("CODE")]
        public CustomerCode Code { get; set; }

        [Column("DISCOUNT")]
        public int? Discount { get; set; }

        public virtual List<Order> Orders { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
