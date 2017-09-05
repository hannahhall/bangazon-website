using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bangazon.Models
{
    public class PaymentType
    {
        [Key]
        public int PaymentTypeId { get;set; }

        [Required]
        [StringLength(12)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}
