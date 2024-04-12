using Microsoft.EntityFrameworkCore;
using Shope.Api.Extensions.Endpoints;
using Shope.Application.Base.Database;

namespace Shope.Api.Endpoints;

public class CustomerEndpoints : IEndpointBuilder
{
    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/customers", async (IShopeeContext context) =>
        {
            return await context.Customers.ToListAsync();
        });
    }
}