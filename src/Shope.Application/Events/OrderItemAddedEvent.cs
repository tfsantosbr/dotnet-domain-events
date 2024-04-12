using Shope.Application.Base.Domain;

namespace Shope.Application.Events;

public record OrderItemAddedEvent(
    Guid OrderItemId,
    Guid OrderId,
    Guid ProductId,
    int ProductQuantity) : DomainEvent;