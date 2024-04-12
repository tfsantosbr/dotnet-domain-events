using Microsoft.EntityFrameworkCore;
using Shope.Api.Extensions.Endpoints;
using Shope.Application.Base.Database;

namespace Shope.Api.Endpoints;

public class ProductEndpoints : IEndpointBuilder
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (IShopeeContext context) =>
        {
            return await context.Products.ToListAsync();
        });
    }
}