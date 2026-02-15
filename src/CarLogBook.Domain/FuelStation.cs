namespace CarLogBook.Domain;

public sealed class FuelStation
{
    private readonly List<FuelEntry> _fuelEntries = [];

    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public IReadOnlyCollection<FuelEntry> FuelEntries => _fuelEntries;
}