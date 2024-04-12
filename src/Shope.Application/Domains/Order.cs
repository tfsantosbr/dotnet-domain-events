using Shope.Application.Base.Domain;
using Shope.Application.Events;

namespace Shope.Application.Domains;

public class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = [];

    public Order(Guid customerId, Guid? id = null)
    {
        Id = id ?? default;
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
    public Customer Customer { get; private set; } = null!;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public OrderStatus Status { get; set; }
    public bool IsConfirmed => Status == OrderStatus.Confirmed;

    public OrderItem AddItem(Guid productId, int quantity)
    {
        var orderItem = new OrderItem(productId, quantity);

        _items.Add(orderItem);

        RaiseEvent(new OrderItemAddedEvent(
            OrderItemId: orderItem.Id, 
            OrderId: orderItem.OrderId, 
            ProductId: orderItem.ProductId, 
            ProductQuantity: orderItem.Quantity));

        return orderItem;
    }

    public void RemoveItem(Guid itemId)
    {
        var orderItem = Items.FirstOrDefault(x => x.Id == itemId);

        if (orderItem is null)
        {
            throw new Exception("Item not found");
        }

        _items.Remove(orderItem);

        RaiseEvent(new OrderItemRemovedEvent(
            OrderItemId: orderItem.Id, 
            OrderId: orderItem.OrderId, 
            ProductId: orderItem.ProductId, 
            ProductQuantity: orderItem.Quantity));
    }

    public void Confirm()
    {
        Status = OrderStatus.Confirmed;

        RaiseEvent(new OrderConfirmedEvent(
            OrderId: Id,
            CustomerId: Customer.Id, 
            CustomerName: Customer.Name, 
            CustomerEmail: Customer.Email,
            OrderStatus: Status));
    }   
}

public enum OrderStatus
{
    Created,
    Confirmed
}
