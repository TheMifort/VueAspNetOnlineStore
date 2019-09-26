using OnlineStore.Areas.Items.Models.Response.Items;

namespace OnlineStore.Areas.Items.Models.Response.Order
{
    public class OrderItemResponseModel
    {
        public ItemResponseModel Item { get; set; }

        public int? ItemsCount { get; set; }

        public decimal? ItemPrice { get; set; }
    }
}