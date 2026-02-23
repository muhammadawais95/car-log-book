using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarLogBook.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CarLogBookDbContext>
{
    public CarLogBookDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CarLogBookDbContext>();
        optionsBuilder.UseSqlite("Data Source=carlogbook.db");

        return new CarLogBookDbContext(optionsBuilder.Options);
    }
}
