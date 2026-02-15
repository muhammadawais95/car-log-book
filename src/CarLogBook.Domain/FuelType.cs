namespace CarLogBook.Domain;

public sealed class FuelType
{
    public required Guid Id { get; init;  }

    public required string Name { get; init; }
}