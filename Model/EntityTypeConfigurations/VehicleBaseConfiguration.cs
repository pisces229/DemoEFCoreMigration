using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Definitions;
using Model.Entities;
using System;

namespace Model.EntityTypeConfigurations;

public class VehicleBaseConfiguration : IEntityTypeConfiguration<VehicleBase>
{
    public void Configure(EntityTypeBuilder<VehicleBase> builder)
    {
        builder.ToTable(t =>
        {
            {
                var checkConstraintName = $"{nameof(VehicleBase.Type)} IN ({(int)VehicleType.SmallCar}, {(int)VehicleType.LargeCar})";
                t.HasCheckConstraint(DbContextUtil.CreateCheckConstraint(nameof(VehicleBase), checkConstraintName), checkConstraintName);
            }
        });

        builder.HasKey(e => e.Id);

        builder.HasDiscriminator(e => e.Type)
            .HasValue<VehicleSmallCar>(VehicleType.SmallCar)
            .HasValue<VehicleLargeCar>(VehicleType.LargeCar);

        builder.Property(e => e.Type).IsRequired();

        builder.Property(e => e.Name).IsRequired();

        builder.HasIndex(e => e.Type);
    }
}
