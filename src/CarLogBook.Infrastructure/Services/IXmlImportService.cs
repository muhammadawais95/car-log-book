namespace CarLogBook.Infrastructure.Services;

public interface IXmlImportService
{
    Task<XmlImportResult> ImportFromFileAsync(string filePath);
}

public sealed class XmlImportResult
{
    public List<XmlCarData> Cars { get; init; }

    public List<XmlFuelEntryData> FuelEntries { get; init; }

    public List<XmlMaintenanceEventData> MaintenanceEvents { get; init; }

    public List<XmlFuelTypeData> FuelTypes { get; init; }

    public List<XmlFuelStationData> FuelStations { get; init; }

    public List<XmlMaintenanceCategoryData> MaintenanceCategories { get; init; }

    public XmlImportResult()
    {
        Cars = [];
        FuelEntries = [];
        MaintenanceEvents = [];
        FuelTypes = [];
        FuelStations = [];
        MaintenanceCategories = [];
    }
}

public sealed record XmlCarData(Guid Id, string Name, string? Make, string? Model, string? ManufacturedYear, decimal Cost, decimal OpenMileage, string? LicensePlate, string? RegistrationNumber, bool IsActive);

public sealed record XmlFuelEntryData(Guid Id, Guid CarId, DateTime DateUtc, decimal Odometer, decimal Price, Guid FuelTypeId, Guid FuelStationId, decimal FuelVolume, string? Name);

public sealed record XmlMaintenanceEventData(Guid Id, Guid CarId, DateTime DateUtc, decimal Odometer, decimal Price, Guid CategoryId, string? Name, string? Comment);

public sealed record XmlFuelTypeData(Guid Id, string Name);

public sealed record XmlFuelStationData(Guid Id, string Name);

public sealed record XmlMaintenanceCategoryData(Guid Id, string Name);