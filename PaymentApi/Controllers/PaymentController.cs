using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Model;
using PaymentApi.Service;

namespace PaymentApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatepaymentAsync([FromBody] Payment payment)
    {
        var NewPayment = await _paymentService.CreatepaymentAsync(payment);
        return Ok(payment);
    }
    [HttpGet]
    public async Task<IActionResult> GetPayments()
    {
        try
        {
            var payments = await _paymentService.GetPayMentAsync();
            return Ok(payments);

        }
        catch (Exception ex)
        {

        }

        return BadRequest();
    }
}
