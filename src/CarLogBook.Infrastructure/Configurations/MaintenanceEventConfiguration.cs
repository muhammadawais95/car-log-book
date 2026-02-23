using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarLogBook.Infrastructure.Configurations;

public class MaintenanceEventConfiguration : IEntityTypeConfiguration<MaintenanceEvent>
{
    public void Configure(EntityTypeBuilder<MaintenanceEvent> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.HasOne(x => x.Category)
               .WithMany()
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(x => x.Odometer, o =>
        {
            o.Property(p => p.Kilometers).HasColumnName("OdometerKilometers");
        });

        builder.OwnsOne(x => x.Cost, c =>
        {
            c.Property(m => m.Amount).HasColumnName("CostAmount");
            c.Property(m => m.Currency).HasColumnName("CostCurrency").HasMaxLength(3);
        });
    }
}