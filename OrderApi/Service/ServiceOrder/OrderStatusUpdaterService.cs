using Microsoft.EntityFrameworkCore;
using OrderApi.Data;

namespace OrderApi.Service.ServiceOrder;

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
            await ProcessOrdersAsync(stoppingToken);
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task ProcessOrdersAsync(CancellationToken stoppingToken)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            var ordersToUpdate = await dbContext.Orders
                .Where(o => o.Status == "Đã Thanh Toán" || o.Status == "Chưa Thanh Toán")
                .ToListAsync(stoppingToken);

            if (!ordersToUpdate.Any()) return; 

            foreach (var order in ordersToUpdate)
            {
                order.Status = order.Status == "Đã Thanh Toán" ? "Đã Vận Chuyển" : "Đã Hủy Đơn";
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật Status");
        }
    }
}
