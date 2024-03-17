using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.Data;

public class ApplicationDbContext : IdentityDbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<BookStore.Models.Product> Product { get; set; } = default!;
    public DbSet<BookStore.Models.Category> Category { get; set; } 
    public DbSet<CartItem> CartItems { get; set; }= default!;
    public DbSet<Order> Order { get; set; }= default!;

    public DbSet<OrderItem> OrderItem{ get; set; }= default!;

}