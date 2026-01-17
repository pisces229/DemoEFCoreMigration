using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Definitions;
using Model.Entities;
using Model.JsonObjects;

namespace Model.EntityTypeConfigurations;

public class AppTableConfiguration : IEntityTypeConfiguration<AppTable>
{
    public void Configure(EntityTypeBuilder<AppTable> builder)
    {
        builder.ToTable(t => t.HasComment("AppTable"));

        builder.HasKey(e => e.Id);

        // [Enum]
        // EnumToStringConverter
        //.HasConversion<string>()
        //new EnumToStringConverter<OrderStatus>()

        // [Boolean]
        // new BoolToStringConverter("N", "Y")
        // new BoolToZeroOneConverter<int>()

        // [Objects]
        //builder.Property(e => e.[Class])
        //    .HasConversion(
        //    // Write
        //    v => v.[Property],
        //    // Read
        //    v => new [Class] { [Property] = v }); ;

        // [GUID]
        // new GuidToBytesConverter()
        // new GuidToStringConverter()

        // [JSON]
        //builder.Property(e => e.[Property])
        //    .HasConversion(
        //        v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
        //        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null),
        //        // Option：Configure a Value Comparer so that EF Core can correctly detect changes.
        //        new ValueComparer<List<string>>(
        //            (c1, c2) => c1.SequenceEqual(c2),
        //            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        //            c => c.ToList()));

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
    }
}
