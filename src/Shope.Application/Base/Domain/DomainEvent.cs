namespace Shope.Application.Base.Domain;

public record DomainEvent : IDomainEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
}
