using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
        
    public int CategoryId { get; set; }

    // Thuộc tính thêm
    [StringLength(100)]
    public string Author { get; set; }

    [StringLength(255)] // Thay đổi chiều dài nếu cần
    public string ImageUrl { get; set; }

    [StringLength(500)]  
    public string Language { get; set; }

    public Category? Category { get; set; }
    public virtual ICollection<OrderItem>? OrderItems { get; set; }

}