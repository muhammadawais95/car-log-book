namespace CarLogBook.Domain;

public sealed class OdometerReading 
{
    public decimal Kilometers { get; set; }

    public OdometerReading() { }

    public OdometerReading(decimal kilometers)
    {
        if (kilometers < 0)
            throw new ArgumentOutOfRangeException(nameof(kilometers));

        Kilometers = decimal.Round(kilometers, 1, MidpointRounding.AwayFromZero);
    }
}