using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderApi.Data;
using OrderApi.Exceptions;
using OrderApi.Model;

namespace OrderApi.Service;

public class OrderService : IOrderService
{
    private readonly OrderDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly IValidator<Order> _validator;
    public OrderService(OrderDbContext context, IHttpClientFactory httpClientFactory, IValidator<Order> validator)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient("PaymentClient");
        _validator = validator;
    }
    public class PaymentStatus
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string? Status { get; set; }
    }
    public async Task<string> CreateOrderAsync(Order order)
    {
        var validationResult = await _validator.ValidateAsync(order);
        if (!validationResult.IsValid)
        {
            throw new ModelvalidationException("Dữ liệu không hợp lệ");
        }
        _context.Orders.Add(order);

        var response = await _httpClient.GetAsync("http://localhost:5062/api/Payment");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var statuses = JsonConvert.DeserializeObject<List<PaymentStatus>>(content);

                if (statuses != null && statuses.Any())
                {
                    var random = new Random();
                    string? selectedStatus = statuses[random.Next(statuses.Count)].Status;
                    order.Status = selectedStatus;
                    await _context.SaveChangesAsync();
                    return selectedStatus;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine("JSON deserialization error: " + ex.Message);
                return "Lỗi xử lý dữ liệu từ Payment API";
            }
        }
        order.Status = "Chưa Thanh Toán";
        await _context.SaveChangesAsync();
        return "Chưa Thanh Toán";

    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }


    public async Task<bool> UpdateOrderAsync(Order order)
    {
        var validationResult = await _validator.ValidateAsync(order);
        if (!validationResult.IsValid)
        {
            throw new ModelvalidationException("Dữ liệu không hợp lệ");
        }
        var orders = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        string status = orders.Status;
        orders.CustomerName = order.CustomerName;                     
        orders.EmployeeName = order.EmployeeName;
        orders.InvoiceDate = order.InvoiceDate;
        orders.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        return order;
    }


}
