using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderApi.Model
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string ImageUrl { get; set; }
        [ForeignKey("Category")]
        public int IdCategory { get; set; }
        public Category Category { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
