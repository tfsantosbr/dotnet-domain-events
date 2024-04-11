using Shope.Application.Domains;
using Shope.Infrastructure;

public class ShopeeContextSeed
{
    public static void Seed(ShopeeContext context)
    {
        // Add example Customers
        var customers = new List<Customer>
        {
            new("Customer 1", "customer1@email.com", new Guid("3f8b0c0a-209e-4085-8963-2c764e5a2c13")),
            new("Customer 2", "customer2@email.com", new Guid("6dcd4ce0-4b39-4284-9727-8cc3b1b714e5"))
        };
        context.Customers.AddRange(customers);

        // Add example Products
        var products = new List<Product>
        {
            new("Product 1", 119.99m,100, new Guid("a6a7a954-58a8-4b11-91f5-8f0edf6b5e4a")),
            new("Product 2", 89.99m, 250, new Guid("b2d6f6e1-6e39-46a2-b274-4d24b4d4f065"))
        };
        context.Products.AddRange(products);

        // Add example Orders
        var order = new Order(customers[0].Id, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
        order.AddItem(products[0].Id, 2);
        order.AddItem(products[1].Id, 8);

        var orders = new List<Order>
        {
            order,
            new(customers[1].Id, new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3302"))
        };
        context.Orders.AddRange(orders);

        // Save changes to the database
        context.SaveChanges();
    }
}