using Microsoft.EntityFrameworkCore;
using Shope.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add InMemoryDatabase
builder.Services.AddDbContext<ShopeeContext>(options =>
{
    options.UseInMemoryDatabase("ShopeDatabase");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<ShopeeContext>();
    ShopeeContextSeed.Seed(context!);
}

app.UseHttpsRedirection();

// Products

app.MapGet("/products", async (ShopeeContext context) =>
{
    return await context.Products.ToListAsync();
});

// Customers

app.MapGet("/customers", async (ShopeeContext context) =>
{
    return await context.Customers.ToListAsync();
});

app.Run();
