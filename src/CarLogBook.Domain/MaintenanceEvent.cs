namespace CarLogBook.Domain;

public sealed class MaintenanceEvent
{
    public required Guid Id { get; init; }

    public required Guid CarId { get; init; }

    public required Guid CategoryId { get; init; }

    public DateTime DateUtc { get; set; }

    public OdometerReading Odometer { get; set; }

    public Money Cost { get; set; }

    public string? Notes { get; set; } 
}