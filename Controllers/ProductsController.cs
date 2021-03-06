using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bangazon.Models;
using Bangazon.Data;
using Bangazon.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System;

namespace Bangazon.Controllers
{
    public class ProductsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext ctx, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }

        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Index()
        {
            // Create new instance of the view model
            ProductListViewModel model = new ProductListViewModel();

            // Set the properties of the view model
            model.Products = await _context.Product.ToListAsync(); 
            return View(model);
        }

        public async Task<IActionResult> Detail([FromRoute]int? id)
        {
            // If no id was in the route, return 404
            if (id == null)
            {
                return NotFound();
            }

            // Create new instance of view model
            ProductDetailViewModel model = new ProductDetailViewModel();

            // Set the `Product` property of the view model
            model.Product = await _context.Product
                    .Include(prod => prod.User)
                    .SingleOrDefaultAsync(prod => prod.ProductId == id);

            // If product not found, return 404
            if (model.Product == null)
            {
                return NotFound();
            }

            return View(model); 
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ProductCreateViewModel model = new ProductCreateViewModel(_context);

            // Get current user
            var user = await GetCurrentUserAsync();

            return View(model); 
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            // Remove the user from the model validation because it is
            // not information posted in the form
            ModelState.Remove("product.User");

            if (ModelState.IsValid)
            {
                /*
                    If all other properties validation, then grab the 
                    currently authenticated user and assign it to the 
                    product before adding it to the db _context
                */
                var user = await GetCurrentUserAsync();
                product.User = user;

                _context.Add(product);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ProductCreateViewModel model = new ProductCreateViewModel(_context);
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Purchase(int? id)
        {
            Product product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == id);

            return View(product);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(ProductPurchaseViewModel productVM)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            Product product = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == productVM.ProductId);
            var currentOrder = await _context.Order.SingleOrDefaultAsync(o => o.User == user && o.PaymentTypeId == null);
            if (currentOrder == null)
            {
                currentOrder = new Order();
                currentOrder.User = user;
                currentOrder.CreatedAt = DateTime.Now;
                _context.Add(currentOrder);
                await _context.SaveChangesAsync();
            }
            OrderProducts orderProduct = new OrderProducts()
            {
                OrderId = currentOrder.OrderId,
                ProductId = product.ProductId
            };
            _context.Add(orderProduct);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("ShoppingCart", "Order");
        }

        // [HttpPost]
        public async Task<IActionResult> Search(string query, string search)
        {
            string lowerCaseQuery = query.ToLower();
            List<Product> products = new List<Product>();
            if (search == "product" || search == null)
            {
                products = await _context.Product
                    .Where(p => p.Title
                        .ToLower()
                        .Contains(lowerCaseQuery) 
                        || 
                        p.Description
                        .ToLower()
                        .Contains(lowerCaseQuery))
                    .ToListAsync();
            }
            else if (search == "location")
            {
                products = await _context.Product
                    .Where(p => p.Location.ToLower() == lowerCaseQuery)
                    .ToListAsync();
            }

            return View(products);
        }        

        public async Task<IActionResult> Types()
        {
            var model = new ProductTypesViewModel();
            

            model.ProductTypes = (from product in _context.Product
                group product by product.ProductTypeId into grouped
                join pt in _context.ProductType
                on grouped.Key equals pt.ProductTypeId
                select new ProductTypeWithProducts {  
                    Count = grouped.Count(), 
                    ProductType = pt, 
                    Products = grouped.Take(3).ToArray()
                }).ToList();

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
