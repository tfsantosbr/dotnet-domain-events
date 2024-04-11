using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shope.Api.Requests;
using Shope.Application.Domains;
using Shope.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add InMemoryDatabase
builder.Services.AddDbContext<ShopeeContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("Default"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<ShopeeContext>()!;
    context.Database.Migrate();
}

app.UseHttpsRedirection();

// Orders

app.MapGet("/orders", async (ShopeeContext context) =>
{
    return await context.Orders.ToListAsync();
});

app.MapPost("/orders", async (ShopeeContext context, CreateOrderRequest request) =>
{
    var order = new Order(request.CustomerId);

    context.Orders.Add(order);

    await context.SaveChangesAsync();

    return Results.Created($"/orders/{order.Id}", order);
});

app.MapGet("/orders/{id}", async (ShopeeContext context, Guid id) =>
{
    var order = await context.Orders
        .AsNoTracking()
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == id);

    return order is null ? Results.NotFound() : Results.Ok(order);
});

app.MapPost("/orders/{id}/items", async (ShopeeContext context, Guid id, CreateOrderItemRequest request) =>
{
    var order = await context.Orders
        .Include(o => o.Items)
        .AsTracking()
        .FirstOrDefaultAsync(o => o.Id == id);

    if (order is null)
    {
        return Results.NotFound();
    }

    order.AddItem(request.ProductId, request.Quantity);

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/orders/{id}/items/{itemId}", async (ShopeeContext context, Guid id, Guid itemId) =>
{
    var order = await context.Orders
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == id);

    if (order is null)
    {
        return Results.NotFound();
    }

    order.RemoveItem(itemId);

    await context.SaveChangesAsync();

    return Results.NoContent();
});


app.MapPut("/orders/{id}/confirmation", async (ShopeeContext context, Guid id) =>
{
    var order = await context.Orders
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == id);

    if (order is null)
    {
        return Results.NotFound();
    }

    if (order.IsConfirmed)
    {
        return Results.BadRequest("Order already confirmed");
    }

    order.Confirm();

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
