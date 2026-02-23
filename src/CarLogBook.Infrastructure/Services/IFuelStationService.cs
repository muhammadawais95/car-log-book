using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarLogBook.Infrastructure.Services;

public interface IFuelStationService
{
    Task<IEnumerable<FuelStation>> GetAllAsync();

    Task<FuelStation?> GetByIdAsync(Guid id);

    Task<int> GetCountAsync();

    Task<FuelStation> CreateAsync(FuelStation fuelStation);

    Task<FuelStation> UpdateAsync(FuelStation fuelStation);

    Task DeleteAsync(Guid id);
}

public class FuelStationService : IFuelStationService
{
    private readonly IFuelStationRepository _repository;
    private readonly ILogger<FuelStationService> _logger;

    public FuelStationService(IFuelStationRepository repository, ILogger<FuelStationService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<FuelStation?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting fuel station by ID: {FuelStationId}", id);

        var result = await _repository.GetAll()
            .Include(x => x.FuelEntries)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        _logger.LogInformation("Retrieved fuel station: {FuelStationName}", result?.Name);
        return result;
    }

    public async Task<IEnumerable<FuelStation>> GetAllAsync()
    {
        _logger.LogInformation("Getting all fuel stations");

        var result = await _repository.GetAll().ToListAsync();

        _logger.LogInformation("Retrieved {Count} fuel stations", result.Count);
        return result;
    }

    public async Task<int> GetCountAsync()
    {
        _logger.LogInformation("Getting fuel station count");
                
        var result = await _repository.GetAll().CountAsync();

        _logger.LogInformation("Fuel station count: {Count}", result);
        return result;
    }

    public async Task<FuelStation> CreateAsync(FuelStation fuelStation)
    {
        _logger.LogInformation("Creating new fuel station: {FuelStationName}", fuelStation.Name);

        var result = await _repository.AddAsync(fuelStation);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Fuel station created with ID: {FuelStationId}", fuelStation.Id);
        return result;
    }

    public async Task<FuelStation> UpdateAsync(FuelStation fuelStation)
    {
        _logger.LogInformation("Updating fuel station: {FuelStationId} - {FuelStationName}", fuelStation.Id, fuelStation.Name);

        _repository.Update(fuelStation);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Fuel station updated: {FuelStationId}", fuelStation.Id);
        return fuelStation;
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting fuel station: {FuelStationId}", id);

        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _repository.Remove(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Fuel station deleted: {FuelStationId}", id);
        }
        else
        {
            _logger.LogWarning("Fuel station not found for deletion: {FuelStationId}", id);
        }
    }
}