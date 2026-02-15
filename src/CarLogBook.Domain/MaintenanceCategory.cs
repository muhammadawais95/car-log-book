namespace CarLogBook.Domain;

public sealed class MaintenanceCategory
{
    public List<MaintenanceEvent> _maintenanceEvents = [];
 
    public required Guid Id { get; init;  }

    public required string Name { get; set; }

    public IReadOnlyCollection<MaintenanceEvent> MaintenanceEvents => _maintenanceEvents;
}