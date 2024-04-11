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
    public Guid CustomerId { get; set; }
    public Customer Customer { get; private set; } = null!;
    public List<OrderItem> Items { get; set; } = [];
    public OrderStatus Status { get; set; }
    public bool IsConfirmed => Status == OrderStatus.Confirmed;

    public void AddItem(Guid productId, int quantity)
    {
        var item = new OrderItem(productId, quantity);

        Items.Add(item);
    }

    public void RemoveItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new Exception("Item not found");
        }

        Items.Remove(item);
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
