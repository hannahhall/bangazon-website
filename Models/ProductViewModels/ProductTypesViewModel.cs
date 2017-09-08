using System.Collections.Generic;
using Bangazon.Models;
using Bangazon.Data;

namespace Bangazon.Models.ProductViewModels
{
  public class ProductTypesViewModel
  {
    public IEnumerable<ProductTypeWithProducts> ProductTypes { get; set; }
  }
}