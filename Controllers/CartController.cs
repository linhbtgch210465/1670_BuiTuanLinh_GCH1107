using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;

        public CartController(ApplicationDbContext context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        public IActionResult Index()
        {
            var items = _cart.GetAllCartItems();
            _cart.CartItems = items;

            return View(_cart);
        }


        public IActionResult AddToCart(int id)
        {
            var selectedProduct = GetProductById(id);

            if (selectedProduct != null)
            {
                _cart.AddToCart(selectedProduct, 1);
                _context.SaveChanges(); 

            }

            return RedirectToAction("Index", "home");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var selectedProduct = GetProductById(id);

            if (selectedProduct != null)
            {
                _cart.RemoveFromCart(selectedProduct);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ReduceQuantity(int id)
        {
            var selectedProduct = GetProductById(id);

            if (selectedProduct != null)
            {
                _cart.ReduceQuantity(selectedProduct);
            }

            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(int id)
        {
            var selectedProduct = GetProductById(id);

            if (selectedProduct != null)
            {
                _cart.IncreaseQuantity(selectedProduct);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            _cart.ClearCart();

            return RedirectToAction("Index");
        }

        public Product GetProductById(int id)
        {
            return _context.Product.FirstOrDefault(b => b.Id == id);
        }

    }
}
