
using Microsoft.EntityFrameworkCore;
using Shope.Application.Base;
using Shope.Application.Domains;

namespace Shope.Infrastructure;

public class ShopeeContext : DbContext, IShopeeContext
{
    public ShopeeContext(DbContextOptions<ShopeeContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders").HasKey(x => x.Id);
            entity.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId);
            entity.HasMany(x => x.Items).WithOne().HasForeignKey(i => i.OrderId);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems").HasKey(x => x.Id);
            entity.Property(x => x.Quantity).IsRequired();
            entity.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers").HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Email).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products").HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired();
        });
    }
}