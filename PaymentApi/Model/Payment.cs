namespace PaymentApi.Model;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string Status { get; set; } = null!;
}
