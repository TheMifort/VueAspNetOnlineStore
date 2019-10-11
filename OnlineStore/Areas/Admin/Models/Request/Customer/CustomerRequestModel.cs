using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Admin.Models.Request.Customer
{
    public class CustomerRequestModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public int? Discount { get; set; }
    }
}