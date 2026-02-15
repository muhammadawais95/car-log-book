using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure.Repositories;

public interface IMaintenanceCategoryRepository
{
    IQueryable<MaintenanceCategory> GetAll();

    Task<MaintenanceCategory?> GetByIdAsync(int id);

    Task<MaintenanceCategory> AddAsync(MaintenanceCategory category);

    void Update(MaintenanceCategory category);

    void Remove(MaintenanceCategory category);

    Task SaveChangesAsync();
}

public class MaintenanceCategoryRepository : IMaintenanceCategoryRepository
{
    private readonly CarLogBookDbContext _context;

    public MaintenanceCategoryRepository(CarLogBookDbContext context) => _context = context;

    public IQueryable<MaintenanceCategory> GetAll() => _context.Set<MaintenanceCategory>().AsNoTracking();

    public async Task<MaintenanceCategory?> GetByIdAsync(int id) => await _context.Set<MaintenanceCategory>().FindAsync(id);

    public async Task<MaintenanceCategory> AddAsync(MaintenanceCategory category)
    {
        var result = await _context.Set<MaintenanceCategory>().AddAsync(category);
        return result.Entity;
    }

    public void Update(MaintenanceCategory category) => _context.Set<MaintenanceCategory>().Update(category);

    public void Remove(MaintenanceCategory category) => _context.Set<MaintenanceCategory>().Remove(category);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}