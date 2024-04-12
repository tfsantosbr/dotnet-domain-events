using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shope.Api.Requests;
using Shope.Application.Base.Database;
using Shope.Application.Base.Notifications;
using Shope.Application.Domains;
using Shope.Application.Events;
using Shope.Infrastructure;
using Shope.Infrastructure.Notifications;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<IShopeeContext, ShopeeContext>(options =>
    options.UseInMemoryDatabase("ShopeeDatabase")
);
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<OrderConfirmedEvent>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<ShopeeContext>()!;
    ShopeeContextSeed.Seed(context);
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

app.MapPut("/orders/{id}/confirmation", async (IShopeeContext context, Guid id) =>
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

app.Run();
