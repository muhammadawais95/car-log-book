using CarLogBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarLogBook.Infrastructure.Configurations;

public class MaintenanceCategoryConfiguration : IEntityTypeConfiguration<MaintenanceCategory>
{
    public void Configure(EntityTypeBuilder<MaintenanceCategory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasMany(x => x.MaintenanceEvents)
               .WithOne()
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}