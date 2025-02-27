using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using OrderApi.Data;
using OrderApi.Exceptions;
using OrderApi.Model;
using OrderApi.Service;
using Xunit;

public class OrderServiceTests
{
    private readonly OrderDbContext _context;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<IValidator<Order>> _mockValidator;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new OrderDbContext(options);
        _context.Database.EnsureCreated();

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new List<OrderService.PaymentStatus>
                {
                    new OrderService.PaymentStatus { Id = 1, OrderId = 1, Status = "Đã Thanh Toán" },
                    new OrderService.PaymentStatus { Id = 2, OrderId = 1, Status = "Chưa Thanh Toán" }
                }), Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object);
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _mockValidator = new Mock<IValidator<Order>>();
        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<Order>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _orderService = new OrderService(_context, _mockHttpClientFactory.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Return_Status_From_PaymentApi()
    {
        var order = new Order { Id = 1, CustomerName = "Nguyễn Văn A" };
        var result = await _orderService.CreateOrderAsync(order);
        Assert.NotNull(result);
        Assert.Contains(result, new[] { "Đã Thanh Toán", "Chưa Thanh Toán" });
        var savedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == 1);
        Assert.NotNull(savedOrder);
        Assert.Equal(order.CustomerName, savedOrder.CustomerName);
        Assert.Contains(savedOrder.Status, new[] { "Đã Thanh Toán", "Chưa Thanh Toán" });
    }

    [Fact]
    public async Task CreateOrderAsync_Should_ThrowException_When_OrderIsInvalid()
    {
        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
            new ValidationFailure("CustomerName", "Tên khách hàng không được để trống")
            }));

        var order = new Order { Id = 3, CustomerName = "" };
        await Assert.ThrowsAsync<ModelvalidationException>(async () =>
            await _orderService.CreateOrderAsync(order));
        var savedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == 3);
        Assert.Null(savedOrder); 
    }

}
