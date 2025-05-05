using Microsoft.EntityFrameworkCore;
using OrderApi.Controllers.Validator;
using OrderApi.Data;
using OrderApi.Middleware;
using Serilog;
using FluentValidation;
using OrderApi.Service.ServiceOrder;
using OrderApi.Service.ServiceRole;
using OrderApi.Service.ServiceUser;
using OrderApi.Service.ServiceEmployee;
using OrderApi.Service.ServiceProduct;
using OrderApi.Service.ServiceCategory;
using OrderApi.Exceptions;

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
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddValidatorsFromAssemblyContaining<ModelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<OrderDtoValidatorException>();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers(); 
app.MapRazorPages();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();
