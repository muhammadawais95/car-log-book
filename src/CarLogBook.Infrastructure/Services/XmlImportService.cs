using System.Globalization;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace CarLogBook.Infrastructure.Services;

public sealed class XmlImportService : IXmlImportService
{
    private readonly ILogger<XmlImportService> _logger;
    private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public XmlImportService(ILogger<XmlImportService> logger)
    {
        _logger = logger;
    }

    public async Task<XmlImportResult> ImportFromFileAsync(string filePath)
    {
        _logger.LogInformation("Starting XML import from file: {FilePath}", filePath);
        
        var content = await File.ReadAllTextAsync(filePath);
        var doc = XDocument.Parse(content);

        List<XmlFuelTypeData> fuelTypes = [];
        List<XmlFuelStationData> fuelStations = [];
        List<XmlMaintenanceCategoryData> maintenanceCategories = [];
        List<XmlCarData> cars = [];
        List<XmlFuelEntryData> fuelEntries = [];
        List<XmlMaintenanceEventData> maintenanceEvents = [];

        var dataValue = doc.Root?.Element("DATA_VALUE");
        if (dataValue != null)
        {
            _logger.LogDebug("Parsing DATA_VALUE section");

            fuelTypes = ParseFuelTypes(dataValue);
            fuelStations = ParseFuelStations(dataValue);
            maintenanceCategories = ParseMaintenanceCategories(dataValue);
        }

        var carsElement = doc.Root?.Element("CAR");
        if (carsElement != null)
        {
            _logger.LogDebug("Parsing CAR section");
            cars = ParseCars(carsElement);
        }

        var logElement = doc.Root?.Element("LOG");
        if (logElement != null)
        {
            _logger.LogDebug("Parsing LOG section");
            (fuelEntries, maintenanceEvents) = ParseLogEntries(logElement);
        }

        _logger.LogInformation("XML import completed. Cars: {CarCount}, FuelTypes: {FuelTypeCount}, FuelStations: {FuelStationCount}, FuelEntries: {FuelEntryCount}, MaintenanceCategories: {CategoryCount}, MaintenanceEvents: {MaintenanceEventCount}",
            cars.Count, fuelTypes.Count, fuelStations.Count, fuelEntries.Count, maintenanceCategories.Count, maintenanceEvents.Count);

        return new XmlImportResult
        {
            FuelTypes = fuelTypes,
            FuelStations = fuelStations,
            MaintenanceCategories = maintenanceCategories,
            Cars = cars,
            FuelEntries = fuelEntries,
            MaintenanceEvents = maintenanceEvents
        };
    }

    private static List<XmlFuelTypeData> ParseFuelTypes(XElement dataValue)
    {
        var fuelTypes = new List<XmlFuelTypeData>();
        var fuelTypeItems = dataValue.Elements("ITEM_TAG")
            .Where(e => (string?)e.Attribute("TYPE") == "0")
            .ToList();

        foreach (var item in fuelTypeItems)
        {
            var id = (string?)item.Attribute("_id");
            var name = (string?)item.Attribute("NAME");
            if (id != null && name != null)
            {
                fuelTypes.Add(new XmlFuelTypeData(Guid.Parse($"00000000-0000-0000-0000-{int.Parse(id, CultureInfo.InvariantCulture):D12}"), name));
            }
        }

        return fuelTypes;
    }

    private static List<XmlFuelStationData> ParseFuelStations(XElement dataValue)
    {
        var fuelStations = new List<XmlFuelStationData>();
        var stationItems = dataValue.Elements("ITEM_TAG")
            .Where(e => (string?)e.Attribute("TYPE") == "1")
            .ToList();

        foreach (var item in stationItems)
        {
            var id = (string?)item.Attribute("_id");
            var name = (string?)item.Attribute("NAME");
            if (id != null && name != null)
            {
                fuelStations.Add(new XmlFuelStationData(Guid.Parse($"00000000-0000-0000-0000-{int.Parse(id, CultureInfo.InvariantCulture):D12}"), name));
            }
        }

        return fuelStations;
    }

    private static List<XmlMaintenanceCategoryData> ParseMaintenanceCategories(XElement dataValue)
    {
        var categories = new List<XmlMaintenanceCategoryData>();
        var categoryItems = dataValue.Elements("ITEM_TAG")
            .Where(e => (string?)e.Attribute("TYPE") == "2")
            .ToList();

        foreach (var item in categoryItems)
        {
            var id = (string?)item.Attribute("_id");
            var name = (string?)item.Attribute("NAME");
            if (id != null && name != null)
            {
                categories.Add(new XmlMaintenanceCategoryData(Guid.Parse($"00000000-0000-0000-0000-{int.Parse(id, CultureInfo.InvariantCulture):D12}"), name));
            }
        }

        return categories;
    }

    private static List<XmlCarData> ParseCars(XElement carsElement)
    {
        var cars = new List<XmlCarData>();
        foreach (var item in carsElement.Elements("ITEM_TAG"))
        {
            var uuid = (string?)item.Attribute("UUID");
            var name = (string?)item.Attribute("NAME");
            var make = (string?)item.Attribute("MAKE");
            var model = (string?)item.Attribute("MODEL");
            var manuf = (string?)item.Attribute("MANUF");
            var carCost = (string?)item.Attribute("CAR_COST");
            var openMil = (string?)item.Attribute("OPEN_MIL");
            var regNum = (string?)item.Attribute("REG_NUM");
            var activeFlag = (string?)item.Attribute("ACTIVE_FLAG");

            if (uuid != null && name != null)
            {
                cars.Add(new XmlCarData(
                    Guid.Parse(uuid),
                    name,
                    make,
                    model,
                    manuf,
                    decimal.TryParse(carCost, out var cost) ? cost : 0,
                    decimal.TryParse(openMil, out var mil) ? mil : 0,
                    null,
                    regNum,
                    activeFlag == "1"
                ));
            }
        }

        return cars;
    }

    private static (List<XmlFuelEntryData> fuelEntries, List<XmlMaintenanceEventData> maintenanceEvents) ParseLogEntries(XElement logElement)
    {
        var fuelEntries = new List<XmlFuelEntryData>();
        var maintenanceEvents = new List<XmlMaintenanceEventData>();

        foreach (var item in logElement.Elements("ITEM_TAG"))
        {
            var id = (string?)item.Attribute("_id");
            var dateStr = (string?)item.Attribute("DATE");
            var odometerStr = (string?)item.Attribute("ODOMETER");
            var priceStr = (string?)item.Attribute("PRICE");
            var carIdStr = (string?)item.Attribute("CAR_ID");
            var fuelTypeIdStr = (string?)item.Attribute("FUEL_TYPE_ID");
            var fuelStationIdStr = (string?)item.Attribute("FUEL_STATION_ID");
            var fuelVolumeStr = (string?)item.Attribute("FUEL_VOLUME");
            var typeIdStr = (string?)item.Attribute("TYPE_ID");
            var otherTypeIdStr = (string?)item.Attribute("OTHER_TYPE_ID");
            var name = (string?)item.Attribute("NAME");
            var typeLog = (string?)item.Attribute("TYPE_LOG");
            var comment = (string?)item.Attribute("COMMENT");

            if (id == null || dateStr == null || carIdStr == null)
                continue;

            var timestamp = long.TryParse(dateStr, out var ts) ? ts : 0;
            var dateUtc = Epoch.AddMilliseconds(timestamp);
            var odometer = decimal.TryParse(odometerStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var od) ? od : 0;
            var price = decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var pr) ? pr : 0;
            var carId = int.TryParse(carIdStr, out var carIdInt) ? carIdInt : 0;

            if (typeLog == "0")
            {
                var fuelTypeId = int.TryParse(fuelTypeIdStr, out var ftId) ? ftId : 0;
                var fuelStationId = int.TryParse(fuelStationIdStr, out var fsId) ? fsId : 0;
                var fuelVolume = decimal.TryParse(fuelVolumeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var fv) ? fv : 0;

                fuelEntries.Add(new XmlFuelEntryData(
                    Guid.Parse($"00000000-0000-0000-0000-{int.Parse(id, CultureInfo.InvariantCulture):D12}"),
                    Guid.Parse($"00000000-0000-0000-0000-{carIdInt:D12}"),
                    dateUtc,
                    odometer,
                    price,
                    Guid.Parse($"00000000-0000-0000-0000-{fuelTypeId:D12}"),
                    Guid.Parse($"00000000-0000-0000-0000-{fuelStationId:D12}"),
                    fuelVolume,
                    name
                ));
            }
            else
            {
                var otherTypeId = int.TryParse(otherTypeIdStr, out var otId) ? otId : 0;

                maintenanceEvents.Add(new XmlMaintenanceEventData(
                    Guid.Parse($"00000000-0000-0000-0000-{int.Parse(id, CultureInfo.InvariantCulture):D12}"),
                    Guid.Parse($"00000000-0000-0000-0000-{carIdInt:D12}"),
                    dateUtc,
                    odometer,
                    price,
                    Guid.Parse($"00000000-0000-0000-0000-{otherTypeId:D12}"),
                    name,
                    comment
                ));
            }
        }

        return (fuelEntries, maintenanceEvents);
    }
}