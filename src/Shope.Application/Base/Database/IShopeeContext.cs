using Microsoft.EntityFrameworkCore;
using Shope.Application.Domains;

namespace Shope.Application.Base.Database;

public interface IShopeeContext
{
    DbSet<Customer> Customers { get; set; }
    DbSet<Order> Orders { get; set; }
    DbSet<Product> Products { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
