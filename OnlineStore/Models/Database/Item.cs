using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models.Database
{
    public class Item
    {
        [Required]
        [Column("ID")]
        public Guid Id { get; set; }

        [Required]
        [Column("CODE", TypeName = "nvarchar(12)")]
        public string Code { get; set; }

        [Required]
        [Column("NAME")]
        public string Name { get; set; }

        [Column("PRICE")]
        public decimal Price { get; set; }

        [Column("CATEGORY", TypeName = "nvarchar(30)")]
        public string Category { get; set; }
    }
}
