using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Definitions;
using Model.Entities;
using Model.JsonObjects;
using System;

namespace Model.EntityTypeConfigurations;

public class VehicleLargeCarConfiguration : IEntityTypeConfiguration<VehicleLargeCar>
{
    public void Configure(EntityTypeBuilder<VehicleLargeCar> builder)
    {
        builder.Property(e => e.CarName).HasMaxLength(50);

        builder.Property(e => e.LargeCarName).HasMaxLength(50);

        builder.Property(e => e.Content)
            .HasColumnType(DbColumnType.Jsonb)
            .HasConversion<LargeCarContentJsonObjectConverter>()
            .Metadata.SetValueComparer(new LargeCarContentJsonObjectComparer());

        builder.Ignore(e => e.CarType);
    }
}
