namespace Shope.Application.Domains;

public class Order
{
    private readonly List<OrderItem> _items = [];

    public Order(Guid customerId, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        CustomerId = customerId;
    }

    private Order()
    {
    }
    
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public void AddItem(Guid productId, int quantity)
    {
        var item = new OrderItem(productId, quantity);

        _items.Add(item);
    }

    public void RemoveItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new Exception("Item not found");
        }

        _items.Remove(item);
    }

    public void ClearItems()
    {
        _items.Clear();
    }   
}

public class Customer
{
    public Customer(string name, string email, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Email = email;
    }

    private Customer()
    {
    }

    public Guid Id { get; set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
}

public class OrderItem
{
    public OrderItem(Guid productId, int quantity, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
    }

    private OrderItem()
    {
    }

    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public Product Product { get; private set; } = null!;
}

public class Product
{
    public Product(string name, decimal price, int stock, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
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
