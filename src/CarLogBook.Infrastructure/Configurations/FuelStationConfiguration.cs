using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarLogBook.Infrastructure.Configurations;

public class FuelStationConfiguration : IEntityTypeConfiguration<FuelStation>
{
    public void Configure(EntityTypeBuilder<FuelStation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasMany(x => x.FuelEntries)
               .WithOne()
               .HasForeignKey(x => x.FuelStationId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}