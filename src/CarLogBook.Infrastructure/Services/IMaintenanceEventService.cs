using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public MaintenanceEventService(IMaintenanceEventRepository repository) => _repository = repository;

    public async Task<MaintenanceEvent?> GetByIdAsync(Guid id) =>
        await _repository.GetAll()
                         .Include(x => x.Category)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<MaintenanceEvent>> GetAllAsync() =>
        await _repository.GetAll().ToListAsync();

    public async Task<int> GetCountAsync() =>
        await _repository.GetAll().CountAsync();

    public async Task<MaintenanceEvent> CreateAsync(MaintenanceEvent maintenanceEvent)
    {
        var result = await _repository.AddAsync(maintenanceEvent);
        await _repository.SaveChangesAsync();
        return result;
    }

    public async Task<MaintenanceEvent> UpdateAsync(MaintenanceEvent maintenanceEvent)
    {
        _repository.Update(maintenanceEvent);
        await _repository.SaveChangesAsync();
        return maintenanceEvent;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _repository.Remove(entity);
            await _repository.SaveChangesAsync();
        }
    }
}