using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Exceptions;
using OrderApi.Model;
using OrderApi.Service;

namespace OrderApi.Controllers;

[Route("[controller]")]
[ApiController]
public class OrderController : ControllerBase
{

    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }
    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        var isCreated = await _orderService.CreateOrderAsync(order);
        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order updatedOrder)
    {
        if (id != updatedOrder.Id)
        {
            return BadRequest("ID không khớp.");
        }

        var result = await _orderService.UpdateOrderAsync(updatedOrder);
        if (!result)
        {
            return NotFound("Order không tồn tại.");
        }

        return Ok("Cập nhật Order thành công.");
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var order = await _orderService.DeleteOrderAsync(orderId);
        if (!order) return NotFound(new { message = "Order not found" });
        return Ok(new { message = "Order deleted successfully" });
    }
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderByIdAsync(int orderId)
    {
        _logger.LogInformation("Fetching order with id {OrderId}", orderId);
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            _logger.LogWarning("Order with id {OrderId} not found", orderId);
            throw new NotFoundException();
        }
        return Ok(order);
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new InvalidOperationException("Order không hợp lệ hoặc không tồn tại.");
    }
}


