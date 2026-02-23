namespace CarLogBook.Domain;

public sealed class FuelEntry
{
    public required Guid Id { get; init; }

    public required Guid CarId { get; set; }

    public required Guid FuelTypeId { get; set; }

    public required Guid FuelStationId { get; set; }

    public DateTime DateUtc { get; set; }

    public OdometerReading? Odometer { get; set; }

    public FuelVolume? Volume { get; set; }

    public Money? Cost { get; set; }

    public bool IsFullTank { get; set; }

    public string Notes { get; set; } = String.Empty;

    public FuelType? FuelType { get; set; }

    public FuelStation? FuelStation { get; set; }
}