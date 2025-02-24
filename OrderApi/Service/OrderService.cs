using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderApi.Data;
using OrderApi.Model;

namespace OrderApi.Service;

public class OrderService : IOrderService
{
    private readonly OrderDbContext _context;
    private readonly HttpClient _httpClient;

    public OrderService(OrderDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        // Sử dụng client được cấu hình sẵn có tên "PaymentClient"
        _httpClient = httpClientFactory.CreateClient("PaymentClient");
    }

    // Định nghĩa lớp ánh xạ với JSON
    public class PaymentStatus
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; }
    }

    public async Task<string> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);

        // Gọi API Payment để lấy danh sách trạng thái thanh toán
        var response = await _httpClient.GetAsync("http://localhost:5062/api/Payment");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                // Deserialize JSON thành danh sách các PaymentStatus
                var statuses = JsonConvert.DeserializeObject<List<PaymentStatus>>(content);

                if (statuses != null && statuses.Any())
                {
                    // Chọn ngẫu nhiên 1 dòng từ danh sách
                    var random = new Random();
                    string selectedStatus = statuses[random.Next(statuses.Count)].Status;

                    // Cập nhật trạng thái cho order
                    order.Status = selectedStatus;
                    await _context.SaveChangesAsync();

                    // Trả về trạng thái đã chọn
                    return selectedStatus;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine("JSON deserialization error: " + ex.Message);
                return "Lỗi xử lý dữ liệu từ Payment API";
            }
        }

        // Nếu API không thành công, cập nhật trạng thái mặc định
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
