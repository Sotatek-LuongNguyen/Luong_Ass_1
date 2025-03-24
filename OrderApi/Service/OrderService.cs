using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderApi.Data;
using OrderApi.Exceptions;
using OrderApi.Model;

namespace OrderApi.Service
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IValidator<Order> _validator;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

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

            await _semaphore.WaitAsync();
            try
            {
                _context.Orders.Add(order);
                var response = await _httpClient.GetAsync("http://localhost:5062/api/Payment");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
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
                order.Status = "Chưa Thanh Toán";
                await _context.SaveChangesAsync();
                return "Chưa Thanh Toán";
            }
            finally
            {
                _semaphore.Release();
            }
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

            await _semaphore.WaitAsync();
            try
            {
                var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
                if (existingOrder == null)
                {
                    return false;
                }

                string status = existingOrder.Status;
                //existingOrder.CustomerName = order.CustomerName;
                //existingOrder.EmployeeName = order.EmployeeName;
                //existingOrder.InvoiceDate = order.InvoiceDate;
                //existingOrder.Quantity = order.Quantity;
                //existingOrder.NameProduct = order.NameProduct;
                //existingOrder.Status = status;
                await _context.SaveChangesAsync();
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            await _semaphore.WaitAsync();
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null) return false;

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
