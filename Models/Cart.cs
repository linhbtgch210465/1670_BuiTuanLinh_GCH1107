using BookStore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BookStore.Models;

public class Cart
{

        private readonly ApplicationDbContext _context;

        public Cart(ApplicationDbContext context)
        {
            _context = context;
        }

        public string Id { get; set; }
        public List<CartItem> CartItems { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("Id") ?? Guid.NewGuid().ToString();

            session.SetString("Id", cartId);

            return new Cart(context) { Id = cartId };
        }

        public CartItem GetCartItem(Product book)
        {
            return _context.CartItems.SingleOrDefault(ci =>
                ci.Book.Id == book.Id && ci.CartId == Id);
        }

        public void AddToCart(Product book, int quantity)
        {
            var cartItem = GetCartItem(book);

            if (cartItem == null)
            {
                if (string.IsNullOrEmpty(Id))
                {
                    Id = Guid.NewGuid().ToString();
                }

                cartItem = new CartItem
                {
                    Book = book,
                    Quantity = quantity,
                    CartId = Id
                };

                _context.CartItems.Add(cartItem);
                _context.SaveChanges();
            }
            else
            {
                cartItem.Quantity += quantity;
                _context.SaveChanges();
            }
        }



        public int ReduceQuantity(Product book)
        {
            var cartItem = GetCartItem(book);
            var remainingQuantity = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    remainingQuantity = --cartItem.Quantity;
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }
            }
            _context.SaveChanges();

            return remainingQuantity;
        }

        public int IncreaseQuantity(Product book)
        {
            var cartItem = GetCartItem(book);
            var remainingQuantity = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 0)
                {
                    remainingQuantity = ++cartItem.Quantity;
                }
            }
            _context.SaveChanges();

            return remainingQuantity;
        }

        public void RemoveFromCart(Product book)
        {
            var cartItem = GetCartItem(book);

            if (cartItem != null && !string.IsNullOrEmpty(cartItem.CartId))
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }
        }


        public void ClearCart()
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartId == Id);

            if (cartItems != null && cartItems.Any())
            {
                _context.CartItems.RemoveRange(cartItems);
                _context.SaveChanges();
            }
        }


        public List<CartItem> GetAllCartItems()
        {
            if (CartItems == null)
            {
                CartItems = new List<CartItem>();
            }
            CartItems.Clear();

            var cartItems = _context.CartItems
                .Where(ci => ci.CartId == Id)
                .Include(ci => ci.Book)
                .ToList();

            CartItems.AddRange(cartItems);

            return CartItems;
        }
        
        public decimal GetCartTotal()
        {
            return _context.CartItems
                .Where(ci => ci.CartId == Id)
                .Select(ci => ci.Book.Price * ci.Quantity)
                .Sum();
        }
}