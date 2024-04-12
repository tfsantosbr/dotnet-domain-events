using Microsoft.EntityFrameworkCore;
using Shope.Api.Extensions.Endpoints;
using Shope.Api.Requests;
using Shope.Application.Base.Database;

namespace Shope.Api.Endpoints;

public class OrderItemEndpoints : IEndpointBuilder
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders/{id}/items", async (IShopeeContext context, Guid id, CreateOrderItemRequest request) =>
        {
            var order = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return Results.NotFound();

            var item = order.AddItem(request.ProductId, request.Quantity);

            await context.SaveChangesAsync();

            return Results.Created($"/orders/{item.Id}", item);
        });

        app.MapDelete("/orders/{id}/items/{itemId}", async (IShopeeContext context, Guid id, Guid itemId) =>
        {
            var order = await context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return Results.NotFound();

            order.RemoveItem(itemId);

            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
