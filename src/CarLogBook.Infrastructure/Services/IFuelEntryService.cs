using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure.Services;

public interface IFuelEntryService
{
    Task<IEnumerable<FuelEntry>> GetAllAsync();

    Task<FuelEntry?> GetByIdAsync(Guid id);

    Task<int> GetCountAsync();

    Task<FuelEntry> CreateAsync(FuelEntry fuelEntry);

    Task<FuelEntry> UpdateAsync(FuelEntry fuelEntry);

    Task DeleteAsync(Guid id);
}

public class FuelEntryService : IFuelEntryService
{
    private readonly IFuelEntryRepository _repository;

    public FuelEntryService(IFuelEntryRepository repository) => _repository = repository;

    public async Task<FuelEntry?> GetByIdAsync(Guid id) =>
        await _repository.GetAll()
                         .Include(x => x.FuelType)
                         .Include(x => x.FuelStation)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<FuelEntry>> GetAllAsync() =>
        await _repository.GetAll().ToListAsync();

    public async Task<int> GetCountAsync() =>
        await _repository.GetAll().CountAsync();

    public async Task<FuelEntry> CreateAsync(FuelEntry fuelEntry)
    {
        var result = await _repository.AddAsync(fuelEntry);
        await _repository.SaveChangesAsync();
        return result;
    }

    public async Task<FuelEntry> UpdateAsync(FuelEntry fuelEntry)
    {
        _repository.Update(fuelEntry);
        await _repository.SaveChangesAsync();
        return fuelEntry;
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