using System;
using System.Collections.Generic;
using OnlineStore.Areas.Items.Models.Response.Order;
using OnlineStore.Models.Enums;

namespace OnlineStore.Areas.Items.Models.Request.Order
{
    public class OrderRequestModel
    {
        public virtual List<OrderItemRequestModel> OrderItems { get; set; }
    }
}