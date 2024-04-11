namespace Shope.Application.Domains;

public class Order
{
    private readonly List<OrderItem> _items = [];

    public Order(Guid customerId, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Created;
    }

    private Order()
    {
    }
    
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CustomerId { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public OrderStatus Status { get; private set; }
    public bool IsConfirmed => Status == OrderStatus.Confirmed;

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

    public void Confirm()
    {
        Status = OrderStatus.Confirmed;
    }   
}

public enum OrderStatus
{
    Created,
    Confirmed
}
