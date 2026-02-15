using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure.Services;

public interface IFuelStationService
{
    Task<IEnumerable<FuelStation>> GetAllAsync();

    Task<FuelStation?> GetByIdAsync(Guid id);

    Task<int> GetCountAsync();

    Task<FuelStation> CreateAsync(FuelStation fuelStation);

    Task<FuelStation> UpdateAsync(FuelStation fuelStation);

    Task DeleteAsync(Guid id);
}

public class FuelStationService : IFuelStationService
{
    private readonly IFuelStationRepository _repository;

    public FuelStationService(IFuelStationRepository repository) => _repository = repository;

    public async Task<FuelStation?> GetByIdAsync(Guid id) =>
        await _repository.GetAll()
                         .Include(x => x.FuelEntries)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<FuelStation>> GetAllAsync() =>
        await _repository.GetAll().ToListAsync();

    public async Task<int> GetCountAsync() =>
        await _repository.GetAll().CountAsync();

    public async Task<FuelStation> CreateAsync(FuelStation fuelStation)
    {
        var result = await _repository.AddAsync(fuelStation);
        await _repository.SaveChangesAsync();
        return result;
    }

    public async Task<FuelStation> UpdateAsync(FuelStation fuelStation)
    {
        _repository.Update(fuelStation);
        await _repository.SaveChangesAsync();
        return fuelStation;
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