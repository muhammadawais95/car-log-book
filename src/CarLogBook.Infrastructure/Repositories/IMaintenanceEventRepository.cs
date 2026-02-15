using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure.Repositories;

public interface IMaintenanceEventRepository
{
    IQueryable<MaintenanceEvent> GetAll();

    Task<MaintenanceEvent?> GetByIdAsync(int id);

    Task<MaintenanceEvent> AddAsync(MaintenanceEvent maintenanceEvent);

    void Update(MaintenanceEvent maintenanceEvent);

    void Remove(MaintenanceEvent maintenanceEvent);

    Task SaveChangesAsync();
}

public class MaintenanceEventRepository : IMaintenanceEventRepository
{
    private readonly CarLogBookDbContext _context;

    public MaintenanceEventRepository(CarLogBookDbContext context) => _context = context;

    public IQueryable<MaintenanceEvent> GetAll() => _context.Set<MaintenanceEvent>().AsNoTracking();

    public async Task<MaintenanceEvent?> GetByIdAsync(int id) => await _context.Set<MaintenanceEvent>().FindAsync(id);

    public async Task<MaintenanceEvent> AddAsync(MaintenanceEvent maintenanceEvent)
    {
        var result = await _context.Set<MaintenanceEvent>().AddAsync(maintenanceEvent);
        return result.Entity;
    }

    public void Update(MaintenanceEvent maintenanceEvent) => _context.Set<MaintenanceEvent>().Update(maintenanceEvent);

    public void Remove(MaintenanceEvent maintenanceEvent) => _context.Set<MaintenanceEvent>().Remove(maintenanceEvent);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}