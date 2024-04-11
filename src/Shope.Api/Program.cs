using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shope.Api.Requests;
using Shope.Application.Base;
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
builder.Services.AddDbContext<IShopeeContext, ShopeeContext>(options =>
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

    if (!context.Database.GetAppliedMigrations().Any())
    {
        context.Database.Migrate();
        ShopeeContextSeed.Seed(context);
    }
}

app.UseHttpsRedirection();

// Products

app.MapGet("/products", async (IShopeeContext context) =>
{
    return await context.Products.ToListAsync();
});

// Customers

app.MapGet("/customers", async (IShopeeContext context) =>
{
    return await context.Customers.ToListAsync();
});

// Orders

app.MapGet("/orders", async (IShopeeContext context) =>
{
    return await context.Orders.ToListAsync();
});

app.MapPost("/orders", async (IShopeeContext context, CreateOrderRequest request) =>
{
    var order = new Order(request.CustomerId);

    context.Orders.Add(order);

    await context.SaveChangesAsync();

    return Results.Created($"/orders/{order.Id}", order);
});

app.MapGet("/orders/{id}", async (IShopeeContext context, Guid id) =>
{
    var order = await context.Orders
        .AsNoTracking()
        .Include(o => o.Customer)
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == id);

    return order is null ? Results.NotFound() : Results.Ok(order);
});

app.MapPost("/orders/{id}/items", async (ShopeeContext context, Guid id, CreateOrderItemRequest request) =>
{
    var order = await context.Orders
        .Include(o => o.Customer)
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == id);

    if (order is null)
    {
        return Results.NotFound();
    }

    //var orderItem = new OrderItem(request.ProductId, request.Quantity);
    order.AddItem(new Guid("a6a7a954-58a8-4b11-91f5-8f0edf6b5e4a"), 5);

    //order.AddItem(request.ProductId, request.Quantity);

    //context.Orders.Update(order);

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/orders/{id}/items/{itemId}", async (IShopeeContext context, Guid id, Guid itemId) =>
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


app.MapPut("/orders/{id}/confirmation", async (IShopeeContext context, Guid id) =>
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
