using FleetManager.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManager.DAL.EntityConfigurations;

public class VehicleEntityConfiguration : IEntityTypeConfiguration<VehicleEntity>
{
    public void Configure(EntityTypeBuilder<VehicleEntity> entity)
    {
        entity.ToTable("vehicles");

        entity.HasKey(v => v.Id);

        entity.Property(v => v.LicensePlate)
              .IsRequired()
              .HasMaxLength(20);

        entity.HasIndex(v => v.LicensePlate).IsUnique();

        entity.Property(v => v.Color)
              .IsRequired()
              .HasMaxLength(20);

        entity.Property(v => v.Year)
              .IsRequired();

        entity.Property(v => v.Status)
              .IsRequired();

        entity.Property(v => v.LeasedTo)
              .HasMaxLength(100);

        entity.Property(v => v.Remarks)
              .HasMaxLength(500);
    }
}
