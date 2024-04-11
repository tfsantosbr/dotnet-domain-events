
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shope.Application.Domains;

namespace Shope.Infrastructure.Configurations;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders").HasKey(x => x.Id);
        builder.HasMany(x => x.Items).WithOne().HasForeignKey(i => i.OrderId);
    }
}
