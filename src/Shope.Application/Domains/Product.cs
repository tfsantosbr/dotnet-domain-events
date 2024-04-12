using Shope.Application.Base.Domain;

namespace Shope.Application.Domains;

public class Product : AggregateRoot
{
    public Product(string name, decimal price, int stock, Guid? id = null)
    {
        Id = id ?? default;
        Name = name;
        Price = price;
        Stock = stock;
    }

    private Product()
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    public void IncreaseStock(int quantity)
    {
        Stock += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity > Stock)
        {
            throw new Exception("Insufficient stock");
        }

        Stock -= quantity;
    }
}
