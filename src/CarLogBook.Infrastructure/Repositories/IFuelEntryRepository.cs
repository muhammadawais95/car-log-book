using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure.Repositories;

public interface IFuelEntryRepository
{
    IQueryable<FuelEntry> GetAll();

    Task<FuelEntry?> GetByIdAsync(int id);

    Task<FuelEntry> AddAsync(FuelEntry fuelEntry);

    void Update(FuelEntry fuelEntry);

    void Remove(FuelEntry fuelEntry);

    Task SaveChangesAsync();
}

public class FuelEntryRepository : IFuelEntryRepository
{
    private readonly DbContext _context;

    public FuelEntryRepository(DbContext context) => _context = context;

    public IQueryable<FuelEntry> GetAll() => _context.Set<FuelEntry>().AsNoTracking();    

    public async Task<FuelEntry?> GetByIdAsync(int id) => await _context.Set<FuelEntry>().FindAsync(id);

    public async Task<FuelEntry> AddAsync(FuelEntry fuelEntry)
    {
        var result = await _context.Set<FuelEntry>().AddAsync(fuelEntry);
        return result.Entity;
    }

    public void Update(FuelEntry fuelEntry) => _context.Set<FuelEntry>().Update(fuelEntry);

    public void Remove(FuelEntry fuelEntry) => _context.Set<FuelEntry>().Remove(fuelEntry);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}