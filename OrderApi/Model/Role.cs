using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderApi.Model
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRole { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }
        public ICollection<User>? Users { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
