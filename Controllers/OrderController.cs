using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Controllers
{
    public class OrderController : Controller
    {
        
            private readonly ApplicationDbContext _context;
            private readonly Cart _cart;
            private readonly UserManager<IdentityUser> _userManager;

            public OrderController(ApplicationDbContext context, Cart cart, UserManager<IdentityUser> userManager)
            {
                _context = context;
                _cart = cart;
                _userManager = userManager;
            }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
             var userId = _userManager.GetUserId(User);
            var cartItems = _cart.GetAllCartItems();
            _cart.CartItems = cartItems;

            if (_cart.CartItems.Count == 0)
            {
                ModelState.AddModelError("", "Cart is empty, please add a book first.");
            }

            if (ModelState.IsValid)
            {
                order.UserId = _userManager.GetUserId(User);

                CreateOrder(order);
                _cart.ClearCart();
                return View("CheckoutComplete", order);
            }

            return View(order);
        }


        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }
        
        public IActionResult ManageOrders()
        {
            var orders = _context.Order.Include(o => o.OrderItems).Include(o => o.User).ToList();

            return View(orders);
        }
        public IActionResult Details(int id)
        {
            var order = _context.Order.Include(o => o.OrderItems).ThenInclude(oi => oi.Book).FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;

            // Lấy đối tượng IdentityUser hiện tại
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Gán đối tượng người dùng cho Order
            order.User = currentUser;

            var cartItems = _cart.CartItems;

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem()
                {
                    Quantity = item.Quantity,
                    BookId = item.Book.Id,
                    OrderId = order.Id,
                    Price = item.Book.Price * item.Quantity
                };
                order.OrderItems.Add(orderItem);
                order.OrderTotal += orderItem.Price;
            }
            _context.Order.Add(order);
            _context.SaveChanges();
            
        }

        public IActionResult Delete(int id)
        {
            var order = _context.Order.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            _context.SaveChanges();

            TempData["Message"] = "Order deleted successfully.";

            return RedirectToAction(nameof(ManageOrders));
        }
        
    }
}