using Microsoft.EntityFrameworkCore;
using OrderApi.Model;

namespace OrderApi.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext()
    {

    }
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
