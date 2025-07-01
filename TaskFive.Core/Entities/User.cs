using TaskFive.Core.Enums;

namespace TaskFive.Core.Entities;

public class User
{
    public string? Id { get; set; }

    public Gender Gender { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public Address Address { get; set; } = new Address();

    public string? AddressString { get; set; }

    public string? PhoneNumber { get; set; }
}
