using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int? PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        public ICollection<OrderProducts> OrderProducts { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}