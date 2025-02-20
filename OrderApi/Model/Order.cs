using System.ComponentModel.DataAnnotations;

namespace OrderApi.Model;

public class Order
{
    [Key]
    //[JsonIgnore]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string EmployeeName { get; set; }

    [Required]
    public DateTime InvoiceDate { get; set; }

    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; }

    [Required]
    [StringLength(50)]
    //[JsonIgnore]
    public string Status { get; set; }
}
