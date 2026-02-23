namespace CarLogBook.Domain;

public sealed class FuelVolume
{
    public decimal Liters { get; set; }

    public FuelVolume() { }

    public FuelVolume(decimal liters)
    {
        if (liters <= 0)
            throw new ArgumentOutOfRangeException(nameof(liters));

        Liters = decimal.Round(liters, 3, MidpointRounding.AwayFromZero);
    }
}