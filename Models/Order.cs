using Microsoft.AspNetCore.Identity;

namespace BookStore.Models;

public class Order
{
    public int Id { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
    public decimal OrderTotal { get; set; }
    public DateTime OrderPlaced { get; set; }
    
    public string? UserId { get; set; }
    public IdentityUser? User { get; set; }

}