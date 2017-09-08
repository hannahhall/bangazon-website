namespace Bangazon.Models
{
    public class ProductTypeWithProducts
    {
        public ProductType ProductType { get; set; }
        public Product[] Products { get; set; }

        public int Count { get; set; }
    }
}