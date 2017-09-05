using System.Collections.Generic;

namespace Bangazon.Models.OrderViewModels.ShoppingCart
{
    public class ShoppingCartViewModel
    {
        public Order Order { get; set; }
        public HashSet<Product> Products { get; set; }

        public ShoppingCartViewModel()
        {
            Products = new HashSet<Product>();
        }
    }
}