using System;
using System.Collections.Generic;
using OnlineStore.Models.Enums;

namespace OnlineStore.Areas.Items.Models.Response.Order
{
    public class OrderResponseModel
    {
        public Guid Id { get; set; }
        
        public DateTime? OrderDate { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public int OrderNumber { get; set; }

        public OrderStatus Status { get; set; }

        public virtual List<OrderItemResponseModel> OrderItems { get; set; }
    }
}