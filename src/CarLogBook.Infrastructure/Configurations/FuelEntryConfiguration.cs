using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarLogBook.Infrastructure.Configurations;

public class FuelEntryConfiguration : IEntityTypeConfiguration<FuelEntry>
{
    public void Configure(EntityTypeBuilder<FuelEntry> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.HasOne(x => x.FuelType)
               .WithMany()
               .HasForeignKey(x => x.FuelTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FuelStation)
               .WithMany()
               .HasForeignKey(x => x.FuelStationId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}