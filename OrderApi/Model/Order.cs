using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderApi.Model;

public class Order
{
    [Key]
    public int Id { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public string? Status { get; set; }
    [ForeignKey("User")]
    public int IdUser { get; set; }
    public User? User { get; set; }  // Navigation Property

    [ForeignKey("Employee")]
    public int IdEmployee { get; set; }
    public Employee? Employee { get; set; }
    
    [ForeignKey("Product")]
    public int IdProduct { get; set; }
    public Product? Product { get; set; }


}
