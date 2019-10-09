using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models.Database
{
    public class OrderItem
    {
        [Required]
        [Column("ID")]
        public Guid Id { get; set; }

        [Required]
        [Column("ORDER_ID")]
        public Guid OrderId { get; set; }

        [Required]
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [Required]
        [Column("ITEM_ID")]
        public Guid ItemId { get; set; }

        [Required]
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [Required]
        [Column("ITEMS_COUNT")]
        public int? ItemsCount { get; set; }

        [Required]
        [Column("ITEM_PRICE")]
        public decimal? ItemPrice { get; set; }
    }
}
