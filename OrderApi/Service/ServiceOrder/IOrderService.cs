using OrderApi.Dto;
using OrderApi.Model;

namespace OrderApi.Service.ServiceOrder
{
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(OrderDto orderDto);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderAsync(OrderDto orderDto);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}
