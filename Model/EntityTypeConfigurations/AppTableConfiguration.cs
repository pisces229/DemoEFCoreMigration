using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model.Definitions;
using Model.Entities;
using Model.JsonObjects;

namespace Model.EntityTypeConfigurations;

public class AppTableConfiguration : IEntityTypeConfiguration<AppTable>
{
    public void Configure(EntityTypeBuilder<AppTable> builder)
    {
        builder.ToTable(t => t.HasComment(nameof(AppTable)));

        builder.ToTable(t => t.HasCheckConstraint(
            DbContextUtil.CreateCheckConstraint(nameof(AppTable), nameof(AppTable.Int)),
            $"{DbContextUtil.ToSnakeCase(nameof(AppTable.Int))} > 0"
        ));

        builder.HasKey(e => e.Id);

        // [Enum]
        // EnumToStringConverter
        //.HasConversion<string>()
        //new EnumToStringConverter<OrderStatus>()

        // [Boolean]
        // new BoolToStringConverter("N", "Y")
        // new BoolToZeroOneConverter<int>()

        // [GUID]
        // new GuidToBytesConverter()
        // new GuidToStringConverter()

        builder.Property(e => e.Enum);
        builder.Property(e => e.String).HasMaxLength(100);
        builder.Property(e => e.Int);
        builder.Property(e => e.Long);
        builder.Property(e => e.Decimal).HasPrecision(5, 3).HasConversion<decimal>();
        builder.Property(e => e.DateOnly);
        builder.Property(e => e.DateTime).HasColumnType(DbColumnType.TimestampWithTimeZone);
        //builder.Property(e => e.DateTime).HasColumnType(DbColumnType.TimestampWithoutTimeZone);
        builder.Property(e => e.DateTimeOffset).HasColumnType(DbColumnType.TimestampWithTimeZone);
        //builder.Property(e => e.DateTimeOffset).HasColumnType(DbColumnType.TimestampWithoutTimeZone);
        builder.Property(e => e.ValueJsonObject)
            .HasColumnType(DbColumnType.Jsonb)
            .HasConversion<ValueJsonObjectConverter>()
            .Metadata.SetValueComparer(new ValueJsonObjectComparer());
        builder.Property(e => e.ValueJsonObjects)
            .HasColumnType(DbColumnType.Jsonb)
            .HasConversion<ValueJsonObjectListConverter>()
            .Metadata.SetValueComparer(new ValueJsonObjectListComparer());
        // SqlServer Concurrency Token
        //builder.Property(e => e.RowVersion)
        //    .HasColumnType(DbColumnType.Timestamp)
        //    .IsRequired()
        //    .IsConcurrencyToken();
        // PostgreSQL Concurrency Token
        builder.Property(e => e.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.HasIndex(e => e.String)
            .IsUnique()
            .HasFilter($"{DbContextUtil.ToSnakeCase(nameof(AppTable.String))} IS NOT NULL");
    }
}
