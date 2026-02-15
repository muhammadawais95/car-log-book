using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarLogBook.Infrastructure.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasMany(x => x.FuelEntries)
               .WithOne()
               .HasForeignKey(x => x.CarId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.MaintenanceEvents)
               .WithOne()
               .HasForeignKey(x => x.CarId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}