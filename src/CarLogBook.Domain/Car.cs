namespace CarLogBook.Domain;

public sealed class Car
{
    private readonly List<FuelEntry> _fuelEntries = [];

    private readonly List<MaintenanceEvent> _maintenanceEvents = [];

    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public EngineType Type { get; set; }

    public string? LicensePlate { get; set; }

    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public string? Year { get; set; }

    public bool IsArchived { get; set; }

    public IReadOnlyCollection<FuelEntry> FuelEntries => _fuelEntries;

    public IReadOnlyCollection<MaintenanceEvent> MaintenanceEvents => _maintenanceEvents;
}