using Microsoft.EntityFrameworkCore;
using OrderApi.Controllers.Validator;
using OrderApi.Data;
using OrderApi.Middleware;
using OrderApi.Service;
using Serilog;
using System.Text.Json;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
/*builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);  // Lắng nghe trên tất cả các IP
});*/
builder.WebHost.UseSentry(o =>
{
    o.Dsn = "https://<your-dsn>@sentry.io/<project-id>";
    o.Debug = true; // Cho biết log debug của Sentry (chỉ dùng trong development)
    o.TracesSampleRate = 1.0; // Thu thập 100% traces (có thể giảm trong production)
});
builder.Services.AddHttpClient();
builder.Services.AddHostedService<OrderStatusUpdaterService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddValidatorsFromAssemblyContaining<ModelValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();
