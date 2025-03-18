using Microsoft.EntityFrameworkCore;
using OrderApi.Controllers.Validator;
using OrderApi.Data;
using OrderApi.Middleware;
using OrderApi.Service;
using Serilog;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<OrderStatusUpdaterService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddValidatorsFromAssemblyContaining<ModelValidator>();

// 🔹 Chỉ sử dụng API và Razor Pages (KHÔNG có View)
builder.Services.AddControllers(); 
builder.Services.AddRazorPages(); // ⚡ Thêm Razor Pages

// 🔹 Swagger (Chỉ hiển thị khi truy cập `/swagger`)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// 🔹 Sử dụng file tĩnh (CSS, JS...)
app.UseStaticFiles();

// 🔹 Định tuyến
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

// 🔹 Middleware xử lý lỗi
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

// 🔹 Định tuyến API
app.MapControllers(); // 🛑 Không có `MapControllerRoute()`

// 🔹 Map Razor Pages
app.MapRazorPages(); // ⚡ Giữ Razor Pages hoạt động

// 🔹 Chỉ bật Swagger khi truy cập `/swagger`
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = "swagger"; // Swagger chỉ hiển thị khi vào /swagger
});

app.Run();
