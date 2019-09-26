using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineStore.Models;

namespace OnlineStore.Areas.Admin.Models.Response.Customer
{
    public class CustomerResponseModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public int? Discount { get; set; }
    }
}