using Shope.Application.Base.Domain;
using Shope.Application.Domains;

namespace Shope.Application.Events;

public record OrderItemAddedEvent : DomainEvent
{
    public OrderItemAddedEvent(OrderItem addedOrdemItem)
    {
        AddedOrdemItem = addedOrdemItem;
    }

    public OrderItem AddedOrdemItem { get; private set; }
}
