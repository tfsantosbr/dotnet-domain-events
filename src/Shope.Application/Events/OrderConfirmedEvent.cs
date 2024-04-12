using Shope.Application.Base.Domain;
using Shope.Application.Domains;

namespace Shope.Application.Events;

public record OrderConfirmedEvent(
    Guid OrderId,
    Guid CustomerId,
    string CustomerName,
    string CustomerEmail,
    OrderStatus OrderStatus
) : DomainEvent;
