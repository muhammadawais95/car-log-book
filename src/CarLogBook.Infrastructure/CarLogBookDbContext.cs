using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarLogBook.Infrastructure;

public sealed class CarLogBookDbContext : DbContext
{
    public DbSet<Car> Cars => Set<Car>();

    public DbSet<FuelEntry> FuelEntries => Set<FuelEntry>();

    public DbSet<FuelStation> FuelStations => Set<FuelStation>();

    public DbSet<FuelType> FuelTypes => Set<FuelType>();

    public DbSet<MaintenanceCategory> MaintenanceCategories => Set<MaintenanceCategory>();

    public DbSet<MaintenanceEvent> MaintenanceEvents => Set<MaintenanceEvent>();

    public CarLogBookDbContext(DbContextOptions<CarLogBookDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarLogBookDbContext).Assembly);
    }
}