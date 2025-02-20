using Microsoft.EntityFrameworkCore;
using OrderApi.Data;

namespace OrderApi.Service;

public class OrderStatusUpdaterService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);
    private readonly ILogger<OrderStatusUpdaterService> _logger;

    public OrderStatusUpdaterService(IServiceScopeFactory scopeFactory, ILogger<OrderStatusUpdaterService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                    var ordersToUpdate = await dbContext.Orders
                        .Where(o => o.Status == "Đã Thanh Toán" || o.Status == "Chưa Thanh Toán")
                        .ToListAsync(stoppingToken);
                    foreach (var order in ordersToUpdate)
                    {
                        if (order.Status == "Đã Thanh Toán")
                        {
                            order.Status = "Đã Vận Chuyển";
                        }
                        else if (order.Status == "Chưa Thanh Toán")
                        {
                            order.Status = "Đã Hủy Đơn";
                        }
                    }

                    if (ordersToUpdate.Any())
                    {
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                _logger.LogError(ex, "Lỗi khi cập nhật Status");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
