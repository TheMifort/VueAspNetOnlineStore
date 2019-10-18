using System;

namespace OnlineStore.Areas.Items.Models.Request.Order
{
    public class ConfirmOrderRequestModel
    {
        public bool Complete { get; set; }
        public DateTime? ShipmentDate { get; set; }
    }
}