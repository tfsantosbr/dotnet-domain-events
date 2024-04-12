using Shope.Application.Base.Domain;
using Shope.Application.Domains;

namespace Shope.Application.Events;

public record OrderConfirmedEvent(
    Guid OrderId,
    DateTime OrderCreatedAt,
    IEnumerable<OrderConfirmedEventItem> OrderItems,
    Guid CustomerId,
    OrderConfirmedEventStatus OrderStatus,
    string CustomerName,
    string CustomerEmail
) : DomainEvent;

public record OrderConfirmedEventItem(
    Guid Id,
    Guid ProductId,
    int Quantity
);

public enum OrderConfirmedEventStatus
{
    Created,
    Confirmed
}