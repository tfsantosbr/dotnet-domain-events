using Shope.Application.Base.Domain;

namespace Shope.Application.Domains;

public class Customer : AggregateRoot
{
    public Customer(string name, string email, Guid? id = null)
    {
        Id = id ?? default;
        Name = name;
        Email = email;
    }

    private Customer()
    {
    }

    public Guid Id { get; set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
}
