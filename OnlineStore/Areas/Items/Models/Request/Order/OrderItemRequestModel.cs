using System;

namespace OnlineStore.Areas.Items.Models.Request.Order
{
    public class OrderItemRequestModel
    {
        public Guid ItemId { get; set; }

        public int? ItemsCount { get; set; }
    }
}