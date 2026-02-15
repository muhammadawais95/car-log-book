using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure.Repositories;

public interface IFuelStationRepository
{
    IQueryable<FuelStation> GetAll();

    Task<FuelStation?> GetByIdAsync(int id);

    Task<FuelStation> AddAsync(FuelStation fuelStation);

    void Update(FuelStation fuelStation);

    void Remove(FuelStation fuelStation);

    Task SaveChangesAsync();
}

public class FuelStationRepository : IFuelStationRepository
{
    private readonly CarLogBookDbContext _context;

    public FuelStationRepository(CarLogBookDbContext context) => _context = context;

    public IQueryable<FuelStation> GetAll() => _context.Set<FuelStation>().AsNoTracking();

    public async Task<FuelStation?> GetByIdAsync(int id) => await _context.Set<FuelStation>().FindAsync(id);

    public async Task<FuelStation> AddAsync(FuelStation fuelStation)
    {
        var result = await _context.Set<FuelStation>().AddAsync(fuelStation);
        return result.Entity;
    }

    public void Update(FuelStation fuelStation) => _context.Set<FuelStation>().Update(fuelStation);

    public void Remove(FuelStation fuelStation) => _context.Set<FuelStation>().Remove(fuelStation);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}