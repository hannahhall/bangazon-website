using System.Collections.Generic;

namespace Bangazon.Models.ProductViewModels
{
    public class ProductSearchViewModel
    {
        public bool ProductSearch { get; set; }
        public bool LocationSearch { get; set; }

        public string Query {get; set; }

        // public List<Product> Products { get; set; }

        // public ProductSearchViewModel () 
        // {
        //     Products = new List<Product>();
        // }
    }
}