using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure.Repositories;

public interface IFuelTypeRepository
{
    IQueryable<FuelType> GetAll();

    Task<FuelType?> GetByIdAsync(Guid id);

    Task<FuelType> AddAsync(FuelType fuelType);

    void Update(FuelType fuelType);

    void Remove(FuelType fuelType);

    Task SaveChangesAsync();
}

public class FuelTypeRepository : IFuelTypeRepository
{
    private readonly CarLogBookDbContext _context;

    public FuelTypeRepository(CarLogBookDbContext context) => _context = context;

    public IQueryable<FuelType> GetAll() => _context.Set<FuelType>().AsNoTracking();

    public async Task<FuelType?> GetByIdAsync(Guid id) => await _context.Set<FuelType>().FindAsync(id);

    public async Task<FuelType> AddAsync(FuelType fuelType)
    {
        var result = await _context.Set<FuelType>().AddAsync(fuelType);
        return result.Entity;
    }

    public void Update(FuelType fuelType) => _context.Set<FuelType>().Update(fuelType);

    public void Remove(FuelType fuelType) => _context.Set<FuelType>().Remove(fuelType);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
