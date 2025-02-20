using Microsoft.EntityFrameworkCore;
using PaymentApi.Data;
using PaymentApi.Model;

namespace PaymentApi.Service;

public class PaymentService
{
    private readonly PaymentDbContext _paymentDbContext;

    public PaymentService(PaymentDbContext paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }
    public async Task<bool> CreatepaymentAsync(Payment payment)
    {
        _paymentDbContext.Payments.Add(payment);
        await _paymentDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Payment>> GetPayMentAsync()
    {
        return await _paymentDbContext.Payments.ToListAsync();
    }

    public async Task<bool> IsOrderConfirmedAsync(int orderId)
    {
        return await _paymentDbContext.Payments.AnyAsync(x => x.OrderId == orderId);
    }
}
