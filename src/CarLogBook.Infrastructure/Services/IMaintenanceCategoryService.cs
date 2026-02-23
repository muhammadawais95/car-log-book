using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarLogBook.Infrastructure.Services;

public interface IMaintenanceCategoryService
{
    Task<IEnumerable<MaintenanceCategory>> GetAllAsync();

    Task<MaintenanceCategory?> GetByIdAsync(Guid id);

    Task<int> GetCountAsync();

    Task<MaintenanceCategory> CreateAsync(MaintenanceCategory category);

    Task<MaintenanceCategory> UpdateAsync(MaintenanceCategory category);

    Task DeleteAsync(Guid id);
}

public class MaintenanceCategoryService : IMaintenanceCategoryService
{
    private readonly IMaintenanceCategoryRepository _repository;
    private readonly ILogger<MaintenanceCategoryService> _logger;

    public MaintenanceCategoryService(IMaintenanceCategoryRepository repository, ILogger<MaintenanceCategoryService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<MaintenanceCategory?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting maintenance category by ID: {CategoryId}", id);

        var result = await _repository.GetAll()
            .Include(x => x.MaintenanceEvents)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        _logger.LogInformation("Retrieved maintenance category: {CategoryName}", result?.Name);
        return result;
    }

    public async Task<IEnumerable<MaintenanceCategory>> GetAllAsync()
    {
        _logger.LogInformation("Getting all maintenance categories");

        var result = await _repository.GetAll().ToListAsync();

        _logger.LogInformation("Retrieved {Count} maintenance categories", result.Count);
        return result;
    }

    public async Task<int> GetCountAsync()
    {
        _logger.LogInformation("Getting maintenance category count");
                
        var result = await _repository.GetAll().CountAsync();

        _logger.LogInformation("Maintenance category count: {Count}", result);
        return result;
    }

    public async Task<MaintenanceCategory> CreateAsync(MaintenanceCategory category)
    {
        _logger.LogInformation("Creating new maintenance category: {CategoryName}", category.Name);

        var result = await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Maintenance category created with ID: {CategoryId}", category.Id);
        return result;
    }

    public async Task<MaintenanceCategory> UpdateAsync(MaintenanceCategory category)
    {
        _logger.LogInformation("Updating maintenance category: {CategoryId} - {CategoryName}", category.Id, category.Name);

        _repository.Update(category);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Maintenance category updated: {CategoryId}", category.Id);
        return category;
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting maintenance category: {CategoryId}", id);

        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _repository.Remove(entity);

            await _repository.SaveChangesAsync();
            _logger.LogInformation("Maintenance category deleted: {CategoryId}", id);
        }
        else
        {
            _logger.LogWarning("Maintenance category not found for deletion: {CategoryId}", id);
        }
    }
}