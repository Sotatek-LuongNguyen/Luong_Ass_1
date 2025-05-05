using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderApi.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Status { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
