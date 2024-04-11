namespace Shope.Application.Domains;

public class Customer
{
    public Customer(string name, string email, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
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
