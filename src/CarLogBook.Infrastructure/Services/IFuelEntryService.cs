using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarLogBook.Infrastructure.Services;

public interface IFuelEntryService
{
    Task<IEnumerable<FuelEntry>> GetAllAsync();
    Task<FuelEntry?> GetByIdAsync(Guid id);
    Task<int> GetCountAsync();
    Task<FuelEntry> CreateAsync(FuelEntry fuelEntry);
    Task<FuelEntry> UpdateAsync(FuelEntry fuelEntry);
    Task DeleteAsync(Guid id);
}

public class FuelEntryService : IFuelEntryService
{
    private readonly IFuelEntryRepository _repository;
    private readonly ILogger<FuelEntryService> _logger;

    public FuelEntryService(IFuelEntryRepository repository, ILogger<FuelEntryService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<FuelEntry?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting fuel entry by ID: {FuelEntryId}", id);

        var result = await _repository.GetAll()
            .Include(x => x.FuelType)
            .Include(x => x.FuelStation)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        _logger.LogInformation("Retrieved fuel entry: {FuelEntryId}", result?.Id);
        return result;
    }

    public async Task<IEnumerable<FuelEntry>> GetAllAsync()
    {
        _logger.LogInformation("Getting all fuel entries");

        var result = await _repository.GetAll().ToListAsync();

        _logger.LogInformation("Retrieved {Count} fuel entries", result.Count);
        return result;
    }

    public async Task<int> GetCountAsync()
    {
        _logger.LogInformation("Getting fuel entry count");

        var result = await _repository.GetAll().CountAsync();

        _logger.LogInformation("Fuel entry count: {Count}", result);
        return result;
    }

    public async Task<FuelEntry> CreateAsync(FuelEntry fuelEntry)
    {
        _logger.LogInformation("Creating new fuel entry for car: {CarId}", fuelEntry.CarId);

        var result = await _repository.AddAsync(fuelEntry);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Fuel entry created with ID: {FuelEntryId}", fuelEntry.Id);
        return result;
    }

    public async Task<FuelEntry> UpdateAsync(FuelEntry fuelEntry)
    {
        _logger.LogInformation("Updating fuel entry: {FuelEntryId}", fuelEntry.Id);

        _repository.Update(fuelEntry);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Fuel entry updated: {FuelEntryId}", fuelEntry.Id);
        return fuelEntry;
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting fuel entry: {FuelEntryId}", id);

        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _repository.Remove(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Fuel entry deleted: {FuelEntryId}", id);
        }
        else
        {
            _logger.LogWarning("Fuel entry not found for deletion: {FuelEntryId}", id);
        }
    }
}