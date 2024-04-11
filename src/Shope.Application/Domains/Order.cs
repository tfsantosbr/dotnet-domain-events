namespace Shope.Application.Domains;

public class Order
{
    private List<OrderItem> _items = new List<OrderItem>();

    public Order(Guid customerId, Guid? id = null)
    {
        this.Id = id ?? Guid.NewGuid();
        this.CustomerId = customerId;
    }
    
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
}

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
}

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public Product Product { get; private set; }
}

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
}
