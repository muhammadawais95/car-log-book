namespace CarLogBook.Domain;

public sealed class MaintenanceEvent
{
    public required Guid Id { get; init; }

    public required Guid CarId { get; set; }

    public required Guid CategoryId { get; set; }

    public DateTime DateUtc { get; set; }

    public OdometerReading? Odometer { get; set; }

    public Money? Cost { get; set; }

    public string? Notes { get; set; }

    public MaintenanceCategory? Category { get; set; }
}