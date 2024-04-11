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
