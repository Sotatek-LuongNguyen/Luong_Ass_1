﻿using Microsoft.EntityFrameworkCore;
using PaymentApi.Model;

namespace PaymentApi.Data;
public class PaymentDbContext : DbContext
{
    public PaymentDbContext()
    {

    }
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
