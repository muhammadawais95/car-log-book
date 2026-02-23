using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarLogBook.Infrastructure.Services;

public interface IMaintenanceEventService
{
    Task<IEnumerable<MaintenanceEvent>> GetAllAsync();

    Task<MaintenanceEvent?> GetByIdAsync(Guid id);

    Task<int> GetCountAsync();

    Task<MaintenanceEvent> CreateAsync(MaintenanceEvent maintenanceEvent);

    Task<MaintenanceEvent> UpdateAsync(MaintenanceEvent maintenanceEvent);

    Task DeleteAsync(Guid id);
}

public class MaintenanceEventService : IMaintenanceEventService
{
    private readonly IMaintenanceEventRepository _repository;
    private readonly ILogger<MaintenanceEventService> _logger;

    public MaintenanceEventService(IMaintenanceEventRepository repository, ILogger<MaintenanceEventService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<MaintenanceEvent?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting maintenance event by ID: {MaintenanceEventId}", id);

        var result = await _repository.GetAll()
            .Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        _logger.LogInformation("Retrieved maintenance event: {MaintenanceEventId}", result?.Id);
        return result;
    }

    public async Task<IEnumerable<MaintenanceEvent>> GetAllAsync()
    {
        _logger.LogInformation("Getting all maintenance events");

        var result = await _repository.GetAll().ToListAsync();

        _logger.LogInformation("Retrieved {Count} maintenance events", result.Count);
        return result;
    }

    public async Task<int> GetCountAsync()
    {
        _logger.LogInformation("Getting maintenance event count");

        var result = await _repository.GetAll().CountAsync();

        _logger.LogInformation("Maintenance event count: {Count}", result);
        return result;
    }

    public async Task<MaintenanceEvent> CreateAsync(MaintenanceEvent maintenanceEvent)
    {
        _logger.LogInformation("Creating new maintenance event for car: {CarId}", maintenanceEvent.CarId);

        var result = await _repository.AddAsync(maintenanceEvent);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Maintenance event created with ID: {MaintenanceEventId}", maintenanceEvent.Id);
        return result;
    }

    public async Task<MaintenanceEvent> UpdateAsync(MaintenanceEvent maintenanceEvent)
    {
        _logger.LogInformation("Updating maintenance event: {MaintenanceEventId}", maintenanceEvent.Id);

        _repository.Update(maintenanceEvent);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Maintenance event updated: {MaintenanceEventId}", maintenanceEvent.Id);
        return maintenanceEvent;
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting maintenance event: {MaintenanceEventId}", id);

        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _repository.Remove(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Maintenance event deleted: {MaintenanceEventId}", id);
        }
        else
        {
            _logger.LogWarning("Maintenance event not found for deletion: {MaintenanceEventId}", id);
        }
    }
}