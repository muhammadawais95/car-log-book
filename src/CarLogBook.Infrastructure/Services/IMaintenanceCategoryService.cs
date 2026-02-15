using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public MaintenanceCategoryService(IMaintenanceCategoryRepository repository) => _repository = repository;

    public async Task<MaintenanceCategory?> GetByIdAsync(Guid id) =>
        await _repository.GetAll()
                         .Include(x => x.MaintenanceEvents)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<MaintenanceCategory>> GetAllAsync() =>
        await _repository.GetAll().ToListAsync();

    public async Task<int> GetCountAsync() =>
        await _repository.GetAll().CountAsync();

    public async Task<MaintenanceCategory> CreateAsync(MaintenanceCategory category)
    {
        var result = await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();
        return result;
    }

    public async Task<MaintenanceCategory> UpdateAsync(MaintenanceCategory category)
    {
        _repository.Update(category);
        await _repository.SaveChangesAsync();
        return category;
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