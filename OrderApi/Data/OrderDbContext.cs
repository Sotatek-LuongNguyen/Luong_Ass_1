using Microsoft.EntityFrameworkCore;
using OrderApi.Model;

namespace OrderApi.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext()
    {

    }
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.IdRole)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Role)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.IdRole)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.IdUser)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.IdProduct)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Employee)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.IdEmployee)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.IdCategory)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
