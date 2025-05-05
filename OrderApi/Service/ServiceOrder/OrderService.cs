using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderApi.Data;
using OrderApi.Dto;
using OrderApi.Exceptions;
using OrderApi.Model;

namespace OrderApi.Service.ServiceOrder
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IValidator<OrderDto> _validator;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public OrderService(OrderDbContext context, IHttpClientFactory httpClientFactory, IValidator<OrderDto> validator)
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

        public async Task<string> CreateOrderAsync(OrderDto orderDto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.IdUser == orderDto.IdUser);
            var employeeExists = await _context.Employees.AnyAsync(e => e.IdEmplyee == orderDto.IdEmployee);
            var productExists = await _context.Products.AnyAsync(p => p.IdProduct == orderDto.IdProduct);

            if (!userExists)
                throw new Exception($"User với ID {orderDto.IdUser} không tồn tại.");
            if (!employeeExists)
                throw new Exception($"Employee với ID {orderDto.IdEmployee} không tồn tại.");
            if (!productExists)
                throw new Exception($"Product với ID {orderDto.IdProduct} không tồn tại.");
            var validationResult = await _validator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                throw new ModelvalidationException("Dữ liệu không hợp lệ");
            }

            var order = new Order
            {
                Quantity = orderDto.Quantity,
                Price = orderDto.Price,
                Created = orderDto.Created,
                Status = orderDto.Status,
                IdUser = orderDto.IdUser,
                IdEmployee = orderDto.IdEmployee,
                IdProduct = orderDto.IdProduct
            };

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

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    Quantity = o.Quantity,
                    Price = o.Price,
                    Created = o.Created,
                    Status = o.Status,
                    IdUser = o.IdUser,
                    IdEmployee = o.IdEmployee,
                    IdProduct = o.IdProduct
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderAsync(OrderDto orderDto)
        {
            var validationResult = await _validator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                throw new ModelvalidationException("Dữ liệu không hợp lệ");
            }

            await _semaphore.WaitAsync();
            try
            {
                var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderDto.Id);
                if (existingOrder == null)
                {
                    return false;
                }

                existingOrder.Quantity = orderDto.Quantity;
                existingOrder.Price = orderDto.Price;
                existingOrder.Created = orderDto.Created;
                existingOrder.Status = orderDto.Status;
                existingOrder.IdUser = orderDto.IdUser;
                existingOrder.IdEmployee = orderDto.IdEmployee;
                existingOrder.IdProduct = orderDto.IdProduct;

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
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                Quantity = order.Quantity,
                Price = order.Price,
                Created = order.Created,
                Status = order.Status,
                IdUser = order.IdUser,
                IdEmployee = order.IdEmployee,
                IdProduct = order.IdProduct
            };
        }
    }
}
