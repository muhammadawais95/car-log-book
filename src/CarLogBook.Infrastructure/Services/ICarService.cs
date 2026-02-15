using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure.Services;

public interface ICarService
{
    Task<IEnumerable<Car>> GetAllAsync();

    Task<Car?> GetByIdAsync(Guid id);

    Task<int> GetCountAsync();

    Task<Car> CreateAsync(Car car);

    Task<Car> UpdateAsync(Car car);

    Task DeleteAsync(Guid id);
}

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;

    public CarService(ICarRepository carRepository) => _carRepository = carRepository;

    public async Task<Car?> GetByIdAsync(Guid id) =>
          await _carRepository.GetAll()
                              .Include(x => x.FuelEntries)
                              .Include(x => x.MaintenanceEvents)
                              .AsNoTracking()
                              .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Car>> GetAllAsync() => await _carRepository.GetAll().ToListAsync();

    public async Task<int> GetCountAsync() => await _carRepository.GetAll().CountAsync();

    public async Task<Car> CreateAsync(Car car)
    {
        var result = await _carRepository.AddAsync(car);
        await _carRepository.SaveChangesAsync();
        return result;
    }

    public async Task<Car> UpdateAsync(Car car)
    {
        _carRepository.Update(car);
        await _carRepository.SaveChangesAsync();
        return car;
    }

    public async Task DeleteAsync(Guid id)
    {
        var car = await GetByIdAsync(id);
        if (car != null)
        {
            _carRepository.Remove(car);
            await _carRepository.SaveChangesAsync();
        }
    }
}