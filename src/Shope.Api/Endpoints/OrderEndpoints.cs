using Microsoft.EntityFrameworkCore;
using Shope.Api.Extensions.Endpoints;
using Shope.Api.Requests;
using Shope.Application.Base.Database;
using Shope.Application.Domains;

namespace Shope.Api.Endpoints;

public class OrderEndpoints : IEndpointBuilder
{
    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var ordersBuilder = builder.MapGroup("/orders");

        ordersBuilder.MapGet("/", async (IShopeeContext context) =>
        {
            return await context.Orders.ToListAsync();
        });

        ordersBuilder.MapPost("/", async (IShopeeContext context, CreateOrderRequest request) =>
        {
            var order = new Order(request.CustomerId);

            context.Orders.Add(order);

            await context.SaveChangesAsync();

            return Results.Created($"/orders/{order.Id}", order);
        });

        ordersBuilder.MapGet("/{id}", async (IShopeeContext context, Guid id) =>
        {
            var order = await context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order is null ? Results.NotFound() : Results.Ok(order);
        });

        ordersBuilder.MapPut("/{id}/confirmation", async (IShopeeContext context, Guid id) =>
        {
            var order = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return Results.NotFound();

            if (order.IsConfirmed)
                return Results.BadRequest("Order already confirmed");

            order.Confirm();

            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
