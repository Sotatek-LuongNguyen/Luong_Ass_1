using System.ComponentModel.DataAnnotations;

namespace OrderApi.Model;

public class Order
{
    public int Id { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime InvoiceDate { get; set; }
    public int Quantity { get; set; }
    public string? NameProduct { get; set; }
    public string? CustomerName { get; set; }
    public string? Status { get; set; }
}
