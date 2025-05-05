using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderApi.Model
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        [ForeignKey("Role")]
        public int IdRole { get; set; }
        public Role Role { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
