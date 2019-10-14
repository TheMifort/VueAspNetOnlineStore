using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Items.Models.Request.Items
{
    public class ItemRequestModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }
    }
}