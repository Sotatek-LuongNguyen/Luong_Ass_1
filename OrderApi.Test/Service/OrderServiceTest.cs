//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using FluentValidation;
//using FluentValidation.Results;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Moq.Protected;
//using Newtonsoft.Json;
//using OrderApi.Data;
//using OrderApi.Exceptions;
//using OrderApi.Model;
//using OrderApi.Service.ServiceOrder;
//using Xunit;

//public class OrderServiceTests : IDisposable
//{
//    private readonly OrderDbContext _context;
//    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
//    private readonly Mock<IValidator<Order>> _mockValidator;
//    private readonly OrderService _orderService;

//    public OrderServiceTests()
//    {
//        var options = new DbContextOptionsBuilder<OrderDbContext>()
//            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//            .Options;
//        _context = new OrderDbContext(options);
//        _context.Database.EnsureCreated();

//        // Thiết lập HttpMessageHandler giả lập trả về kết quả từ Payment API
//        var handlerMock = new Mock<HttpMessageHandler>();
//        handlerMock
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>()
//            )
//            .ReturnsAsync(new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = new StringContent(
//                    JsonConvert.SerializeObject(new List<OrderService.PaymentStatus>
//                    {
//                        new OrderService.PaymentStatus { Id = 1, OrderId = 1, Status = "Đã Thanh Toán" },
//                        new OrderService.PaymentStatus { Id = 2, OrderId = 1, Status = "Chưa Thanh Toán" }
//                    }),
//                    Encoding.UTF8,
//                    "application/json")
//            });

//        var httpClient = new HttpClient(handlerMock.Object);
//        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
//        _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

//        _mockValidator = new Mock<IValidator<Order>>();
//        _mockValidator
//            .Setup(v => v.ValidateAsync(It.IsAny<Order>(), default))
//            .ReturnsAsync(new ValidationResult());

//        _orderService = new OrderService(_context, _mockHttpClientFactory.Object, _mockValidator.Object);
//    }

//    // Dọn dẹp tài nguyên sau khi test
//    public void Dispose()
//    {
//        _context.Database.EnsureDeleted();
//        _context.Dispose();
//    }

//    [Fact]
//    public async Task CreateOrderAsync_Should_Return_Status_From_PaymentApi()
//    {
//        // Arrange
//        var order = new Order { Id = 1, CustomerName = "Nguyễn Văn A", EmployeeName = "Emp A", InvoiceDate = DateTime.Now, Quantity = 2, NameProduct = "Product A" };

//        // Act
//        var result = await _orderService.CreateOrderAsync(order);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Contains(result, new[] { "Đã Thanh Toán", "Chưa Thanh Toán" });

//        var savedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == 1);
//        Assert.NotNull(savedOrder);
//        Assert.Equal(order.CustomerName, savedOrder.CustomerName);
//        Assert.Contains(savedOrder.Status, new[] { "Đã Thanh Toán", "Chưa Thanh Toán" });
//    }

//    [Fact]
//    public async Task CreateOrderAsync_Should_ThrowException_When_OrderIsInvalid()
//    {
//        // Arrange: Setup validator trả về lỗi
//        _mockValidator
//            .Setup(v => v.ValidateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
//            {
//                new ValidationFailure("CustomerName", "Tên khách hàng không được để trống")
//            }));

//        var order = new Order { Id = 3, CustomerName = "" };

//        // Act & Assert
//        await Assert.ThrowsAsync<ModelvalidationException>(async () =>
//            await _orderService.CreateOrderAsync(order));
//        var savedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == 3);
//        Assert.Null(savedOrder);
//    }

//    [Fact]
//    public async Task GetAllOrdersAsync_Should_Return_All_Orders()
//    {
//        // Arrange: Thêm 2 order vào DB
//        _context.Orders.AddRange(new List<Order>
//        {
//            new Order { Id = 10, CustomerName = "KH A", EmployeeName = "Emp A", InvoiceDate = DateTime.Now, Quantity = 1, NameProduct = "Prod A", Status = "Đã Thanh Toán" },
//            new Order { Id = 11, CustomerName = "KH B", EmployeeName = "Emp B", InvoiceDate = DateTime.Now, Quantity = 2, NameProduct = "Prod B", Status = "Chưa Thanh Toán" }
//        });
//        await _context.SaveChangesAsync();

//        // Act
//        var orders = await _orderService.GetAllOrdersAsync();

//        // Assert
//        Assert.NotNull(orders);
//        Assert.True(orders.Count >= 2);
//        Assert.Contains(orders, o => o.Id == 10);
//        Assert.Contains(orders, o => o.Id == 11);
//    }

//    [Fact]
//    public async Task UpdateOrderAsync_Should_Update_Order_Except_Status()
//    {
//        // Arrange: Tạo order ban đầu với Status = "Đã Thanh Toán"
//        var originalOrder = new Order
//        {
//            Id = 20,
//            CustomerName = "Original Customer",
//            EmployeeName = "Original Emp",
//            InvoiceDate = DateTime.Now.AddDays(-1),
//            Quantity = 1,
//            NameProduct = "Original Product",
//            Status = "Đã Thanh Toán"
//        };
//        _context.Orders.Add(originalOrder);
//        await _context.SaveChangesAsync();
//        var updatedOrder = new Order
//        {
//            Id = 20,
//            CustomerName = "Updated Customer",
//            EmployeeName = "Updated Emp",
//            InvoiceDate = DateTime.Now,
//            Quantity = 5,
//            NameProduct = "Updated Product",
//            Status = "Chưa Thanh Toán"
//        };
//        var result = await _orderService.UpdateOrderAsync(updatedOrder);
//        Assert.True(result);
//        var savedOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == 20);
//        Assert.NotNull(savedOrder);
//        Assert.Equal("Đã Thanh Toán", savedOrder.Status);
//        Assert.Equal("Updated Customer", savedOrder.CustomerName);
//        Assert.Equal("Updated Emp", savedOrder.EmployeeName);
//        Assert.Equal("Updated Product", savedOrder.NameProduct);
//        Assert.Equal(5, savedOrder.Quantity);
//    }

//    [Fact]
//    public async Task DeleteOrderAsync_Should_Return_True_When_Order_Exists()
//    {
//        var order = new Order { Id = 30, CustomerName = "Customer Delete", EmployeeName = "Emp", InvoiceDate = DateTime.Now, Quantity = 1, NameProduct = "Prod", Status = "Đã Thanh Toán" };
//        _context.Orders.Add(order);
//        await _context.SaveChangesAsync();
//        var result = await _orderService.DeleteOrderAsync(30);
//        Assert.True(result);
//        var deletedOrder = await _context.Orders.FindAsync(30);
//        Assert.Null(deletedOrder);
//    }

//    [Fact]
//    public async Task DeleteOrderAsync_Should_Return_False_When_Order_Does_Not_Exist()
//    {
//        int nonExistingId = 999;
//        var result = await _orderService.DeleteOrderAsync(nonExistingId);
//        Assert.False(result);
//    }

//    [Fact]
//    public async Task GetOrderByIdAsync_Should_Return_Order_When_Exists()
//    {
//        var order = new Order { Id = 40, CustomerName = "Customer Get", EmployeeName = "Emp", InvoiceDate = DateTime.Now, Quantity = 2, NameProduct = "Prod", Status = "Đã Thanh Toán" };
//        _context.Orders.Add(order);
//        await _context.SaveChangesAsync();
//        var result = await _orderService.GetOrderByIdAsync(40);
//        Assert.NotNull(result);
//        Assert.Equal(40, result.Id);
//        Assert.Equal("Customer Get", result.CustomerName);
//    }
//    [Fact]
//    public async Task ThreadSafeTest_UpdateOrder_When_ManyPeople_Update()
//    {
//        var originalOrder = new Order
//        {
//            Id = 20,
//            CustomerName = "Original Customer",
//            EmployeeName = "Original Emp",
//            InvoiceDate = DateTime.Now.AddDays(-1),
//            Quantity = 1,
//            NameProduct = "Original Product",
//            Status = "Đã Thanh Toán"
//        };
//        _context.Orders.Add(originalOrder);
//        await _context.SaveChangesAsync();

//        var update1 = new Order
//        {
//            Id = 20,
//            CustomerName = "User1 Updated",
//            EmployeeName = "User1 Employee",
//            InvoiceDate = DateTime.Now,
//            Quantity = 10,
//            NameProduct = "Product User1",
//            Status = "ShouldNotChange"
//        };

//        var update2 = new Order
//        {
//            Id = 20,
//            CustomerName = "User2 Updated",
//            EmployeeName = "User2 Employee",
//            InvoiceDate = DateTime.Now,
//            Quantity = 20,
//            NameProduct = "Product User2",
//            Status = "ShouldNotChange"
//        };
//        var task1 = Task.Run(() => _orderService.UpdateOrderAsync(update1));
//        var task2 = Task.Run(async () =>
//        {
//            await Task.Delay(50);
//            return await _orderService.UpdateOrderAsync(update2);
//        });

//        await Task.WhenAll(task1, task2);
//        var finalOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == 20);
//        Assert.NotNull(finalOrder);
//    }
//}
