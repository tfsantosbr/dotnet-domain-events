using Shope.Application.Base.Domain;
using Shope.Application.Domains;

namespace Shope.Application.Events;

public record OrderConfirmedEvent : DomainEvent
{
    public OrderConfirmedEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; private set; }
}
