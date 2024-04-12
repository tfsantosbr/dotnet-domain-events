using Shope.Application.Base.Domain;
using Shope.Application.Domains;

namespace Shope.Application.Events;

public record OrderItemRemovedEvent : DomainEvent
{
    public OrderItemRemovedEvent(OrderItem removedOrderItem)
    {
        RemovedOrderItem = removedOrderItem;
    }

    public OrderItem RemovedOrderItem { get; private set; }
}
