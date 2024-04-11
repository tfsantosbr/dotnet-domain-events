namespace Shope.Application.Domains;

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
