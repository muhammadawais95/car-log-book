namespace CarLogBook.Domain;

public sealed class FuelEntry
{
    public required Guid Id { get; init; }

    public required Guid CarId { get; init; }

    public required Guid FuelTypeId { get; init; }

    public required Guid FuelStationId { get; init; }

    public DateTime DateUtc { get; set; }

    public OdometerReading Odometer { get; set; }

    public FuelVolume Volume { get; set; }

    public Money Cost { get; set; }

    public bool IsFullTank { get; set; }

    public string Notes { get; set; } = String.Empty;
}