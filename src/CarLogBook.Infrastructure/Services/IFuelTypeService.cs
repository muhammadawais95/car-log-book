using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarLogBook.Infrastructure.Services;

public interface IFuelTypeService
{
    Task<IEnumerable<FuelType>> GetAllAsync();

    Task<FuelType?> GetByIdAsync(Guid id);
    Task<int> GetCountAsync();

    Task<FuelType> CreateAsync(FuelType fuelType);

    Task<FuelType> UpdateAsync(FuelType fuelType);

    Task DeleteAsync(Guid id);
}

public class FuelTypeService : IFuelTypeService
{
    private readonly DbContext _context;
    private readonly ILogger<FuelTypeService> _logger;

    public FuelTypeService(DbContext context, ILogger<FuelTypeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<FuelType?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting fuel type by ID: {FuelTypeId}", id);

        var result = await _context.Set<FuelType>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        _logger.LogInformation("Retrieved fuel type: {FuelTypeName}", result?.Name);

        return result;
    }

    public async Task<IEnumerable<FuelType>> GetAllAsync()
    {
        _logger.LogInformation("Getting all fuel types");

        var result = await _context.Set<FuelType>().ToListAsync();

        _logger.LogInformation("Retrieved {Count} fuel types", result.Count);
        return result;
    }

    public async Task<int> GetCountAsync()
    {
        _logger.LogInformation("Getting fuel type count");

        var result = await _context.Set<FuelType>().CountAsync();

        _logger.LogInformation("Fuel type count: {Count}", result);
        return result;
    }

    public async Task<FuelType> CreateAsync(FuelType fuelType)
    {
        _logger.LogInformation("Creating new fuel type: {FuelTypeName}", fuelType.Name);

        var result = await _context.Set<FuelType>().AddAsync(fuelType);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Fuel type created with ID: {FuelTypeId}", fuelType.Id);
        return result.Entity;
    }

    public async Task<FuelType> UpdateAsync(FuelType fuelType)
    {
        _logger.LogInformation("Updating fuel type: {FuelTypeId} - {FuelTypeName}", fuelType.Id, fuelType.Name);

        _context.Set<FuelType>().Update(fuelType);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Fuel type updated: {FuelTypeId}", fuelType.Id);
        return fuelType;
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting fuel type: {FuelTypeId}", id);

        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<FuelType>().Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Fuel type deleted: {FuelTypeId}", id);
        }
        else
        {
            _logger.LogWarning("Fuel type not found for deletion: {FuelTypeId}", id);
        }
    }
}