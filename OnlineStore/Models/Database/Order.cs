using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineStore.Models.Enums;

namespace OnlineStore.Models.Database
{
    public class Order
    {
        [Required]
        [Column("ID")]
        public Guid Id { get; set; }

        [Required]
        [Column("CUSTOMER_ID")]
        public Guid CustomerId { get; set; }

        [Required]
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required]
        [Column("ORDER_DATE", TypeName = "date")]
        public DateTime? OrderDate { get; set; }

        [Required]
        [Column("SHIPMENT_DATE",TypeName = "date")]
        public DateTime? ShipmentDate { get; set; }

        [Column("ORDER_NUMBER")]
        public int OrderNumber { get; set; }

        [Column("STATUS")]
        public OrderStatus Status { get; set; }
    }
}
