using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Model;
using OrderApi.Service;

namespace OrderApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{

    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
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
        // Kiểm tra nếu ID trong URL và trong payload không trùng khớp
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
}


