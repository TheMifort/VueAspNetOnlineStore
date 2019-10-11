using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Admin.Models.Request.Customer
{
    public class CustomerRequestModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int? Discount { get; set; }
    }
}