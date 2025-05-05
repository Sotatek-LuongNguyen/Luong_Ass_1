using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Dto;
using OrderApi.Exceptions;
using OrderApi.Service.ServiceOrder;

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

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
    {
        try 
        {
            var result = await _orderService.CreateOrderAsync(orderDto);
            return Ok(new { message = "Tạo order thành công", status = result });
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = ex.Message,
                detail = ex.InnerException?.Message ?? "No inner exception"
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto updatedOrderDto)
    {
        if (id != updatedOrderDto.Id)
        {
            return BadRequest("ID không khớp.");
        }

        var result = await _orderService.UpdateOrderAsync(updatedOrderDto);
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
        if (!order)
        {
            return NotFound(new { message = "Order không tồn tại." });
        }

        return Ok(new { message = "Order xóa thành công." });
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderByIdAsync(int orderId)
    {
        _logger.LogInformation("Đang tìm kiếm order với ID: {OrderId}", orderId);
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            _logger.LogWarning("Không tìm thấy order với ID: {OrderId}", orderId);
            throw new NotFoundException("Order không tồn tại.");
        }

        return Ok(order);
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new InvalidOperationException("Order không hợp lệ hoặc không tồn tại.");
    }
}
