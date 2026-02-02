using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Definitions;
using Model.Entities;
using Model.JsonObjects;

namespace Model.EntityTypeConfigurations;

public class VehicleSmallCarConfiguration : IEntityTypeConfiguration<VehicleSmallCar>
{
    public void Configure(EntityTypeBuilder<VehicleSmallCar> builder)
    {
        builder.Property(e => e.CarName).HasMaxLength(50);

        builder.Property(e => e.SmallCarName).HasMaxLength(50);

        builder.Property(e => e.Content)
            .HasColumnType(DbColumnType.Jsonb)
            .HasConversion<SmallCarContentJsonObjectConverter>()
            .Metadata.SetValueComparer(new SmallCarContentJsonObjectComparer());

        builder.Ignore(e => e.CarType);
    }
}
