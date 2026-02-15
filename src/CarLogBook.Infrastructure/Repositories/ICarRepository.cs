using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure.Repositories;

public interface ICarRepository
{
    IQueryable<Car> GetAll();

    Task<Car?> GetByIdAsync(int id);

    Task<Car> AddAsync(Car car);

    void Update(Car car);

    void Remove(Car car);

    Task SaveChangesAsync();
}

public class CarRepository : ICarRepository
{
    private readonly CarLogBookDbContext _context;

    public CarRepository(CarLogBookDbContext context) => _context = context;

    public IQueryable<Car> GetAll() => _context.Set<Car>().AsNoTracking();

    public async Task<Car?> GetByIdAsync(int id) => await _context.Set<Car>().FindAsync(id);

    public async Task<Car> AddAsync(Car car)
    {
        var result = await _context.Set<Car>().AddAsync(car);
        return result.Entity;
    }

    public void Update(Car car) => _context.Set<Car>().Update(car);

    public void Remove(Car car) => _context.Set<Car>().Remove(car);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}