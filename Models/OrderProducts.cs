using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models
{
    public class OrderProducts
    {
        [Key]
        public int OrderProductsId { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}