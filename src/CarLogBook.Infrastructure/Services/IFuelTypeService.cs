using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public FuelTypeService(DbContext context) => _context = context;

    public async Task<FuelType?> GetByIdAsync(Guid id) =>
        await _context.Set<FuelType>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<FuelType>> GetAllAsync() =>
        await _context.Set<FuelType>().ToListAsync();

    public async Task<int> GetCountAsync() =>
        await _context.Set<FuelType>().CountAsync();

    public async Task<FuelType> CreateAsync(FuelType fuelType)
    {
        var result = await _context.Set<FuelType>().AddAsync(fuelType);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<FuelType> UpdateAsync(FuelType fuelType)
    {
        _context.Set<FuelType>().Update(fuelType);
        await _context.SaveChangesAsync();
        return fuelType;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<FuelType>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}