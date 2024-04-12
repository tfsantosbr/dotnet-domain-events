
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shope.Application.Base.Database;
using Shope.Application.Base.Domain;
using Shope.Application.Domains;

namespace Shope.Infrastructure;

public class ShopeeContext(DbContextOptions<ShopeeContext> options, IPublisher publisher) 
    : DbContext(options), IShopeeContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEvents(cancellationToken);

        return result;
    }

    private async Task PublishDomainEvents(CancellationToken cancellationToken)
    {
        var domainEvents = ChangeTracker.Entries<AggregateRoot>()
            .Select(e=>e.Entity)
            .Where(e => e.DomainEvents.Count != 0)
            .SelectMany(e => 
            {
                var events = e.DomainEvents.ToList();
                e.ClearEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
            await publisher.Publish(domainEvent, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId);
            entity.HasMany(x => x.Items).WithOne().HasForeignKey(i => i.OrderId);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Quantity).IsRequired();
            entity.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Email).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired();
        });
    }
}