using Shope.Application.Base.Domain;
using Shope.Application.Domains;

namespace Shope.Application.Events;

public record OrderItemAddedEvent(
    Guid OrderItemId,
    Guid OrderId,
    Guid ProductId,
    int ProductQuantity) : DomainEvent;