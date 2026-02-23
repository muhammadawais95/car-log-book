using CarLogBook.Domain;
using CarLogBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<CarService> _logger;

    public CarService(ICarRepository carRepository, ILogger<CarService> logger)
    {
        _carRepository = carRepository;
        _logger = logger;
    }

    public async Task<Car?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting car by ID: {CarId}", id);

        var result = await _carRepository.GetAll()
            .Include(x => x.FuelEntries)
            .Include(x => x.MaintenanceEvents)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        _logger.LogInformation("Retrieved car: {CarName}", result?.Name);
        return result;
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        _logger.LogInformation("Getting all cars");

        var result = await _carRepository.GetAll().ToListAsync();

        _logger.LogInformation("Retrieved {Count} cars", result.Count);
        return result;
    }

    public async Task<int> GetCountAsync()
    {
        _logger.LogInformation("Getting car count");

        var result = await _carRepository.GetAll().CountAsync();

        _logger.LogInformation("Car count: {Count}", result);
        return result;
    }

    public async Task<Car> CreateAsync(Car car)
    {
        _logger.LogInformation("Creating new car: {CarName}", car.Name);

        var result = await _carRepository.AddAsync(car);
        await _carRepository.SaveChangesAsync();

        _logger.LogInformation("Car created with ID: {CarId}", car.Id);
        return result;
    }

    public async Task<Car> UpdateAsync(Car car)
    {
        _logger.LogInformation("Updating car: {CarId} - {CarName}", car.Id, car.Name);

        _carRepository.Update(car);
        await _carRepository.SaveChangesAsync();

        _logger.LogInformation("Car updated: {CarId}", car.Id);
        return car;
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting car: {CarId}", id);

        var car = await GetByIdAsync(id);
        if (car != null)
        {
            _carRepository.Remove(car);
            await _carRepository.SaveChangesAsync();

            _logger.LogInformation("Car deleted: {CarId}", id);
        }
        else
        {
            _logger.LogWarning("Car not found for deletion: {CarId}", id);
        }
    }
}