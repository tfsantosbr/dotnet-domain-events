
using Microsoft.EntityFrameworkCore;
using Shope.Application.Domains;
using System.Reflection;

namespace Shope.Infrastructure;

public class ShopeeContext : DbContext
{
    public ShopeeContext(DbContextOptions<ShopeeContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
