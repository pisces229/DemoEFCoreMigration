using Model.JsonObjects;

namespace Model.EntityTypeConfigurations;

public class AppTableConfiguration : IEntityTypeConfiguration<AppTable>
{
    public void Configure(EntityTypeBuilder<AppTable> builder)
    {
        builder.ToTable(t =>
        {
            t.HasComment(nameof(AppTable));
            t.HasCheckConstraint(
                DbContextUtil.CreateCheckConstraint(nameof(AppTable),
                $"{DbContextUtil.NamingConvention(nameof(AppTable.Int))} > 0"),
                $"{DbContextUtil.NamingConvention(nameof(AppTable.Int))} > 0"
            );
            //t.HasTrigger("");
        });

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        // NOTE: Unique constraint is managed via HasIndex below, not HasAlternateKey
        // builder.HasAlternateKey(e => e.String);

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
        builder.Property(e => e.DateTime);
        builder.Property(e => e.DateTimeOffset);

        builder.Property(e => e.AnyJsonString)
            .HasColumnType(DbColumnType.Jsonb);

        // type set to Jsonb
        //builder.Property(e => e.StringJsonObjects)
        //    .HasColumnType(DbColumnType.Jsonb)
        //    .HasConversion<StringCollectionConverter>()
        //    .Metadata.SetValueComparer(new StringCollectionComparer());
        // type set to TextArray, can't use HasConversion and Metadata.SetValueComparer
        builder.Property(e => e.StringJsonObjects)
            .HasColumnType(DbColumnType.TextArray);

        builder.Property(e => e.ValueJsonObject)
            .HasColumnType(DbColumnType.Jsonb)
            .HasConversion<ValueJsonObjectConverter>()
            .Metadata.SetValueComparer(new ValueJsonObjectComparer());
        // if use builder.OwnsOne, need to create index manually
        //builder.OwnsOne(e => e.ValueJsonObject, e =>
        //{
        //    e.ToJson();
        //});

        builder.Property(e => e.ValueJsonObjects)
            .HasColumnType(DbColumnType.Jsonb)
            .HasConversion<ValueJsonObjectListConverter>()
            .Metadata.SetValueComparer(new ValueJsonObjectListComparer());
        // if use builder.OwnsMany, need to create index manually
        //builder.OwnsMany(e => e.ValueJsonObjects, e =>
        //{
        //    e.ToJson();
        //});

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
            .HasFilter($"{DbContextUtil.NamingConvention(nameof(AppTable.String))} IS NOT NULL");

        // only for PostgreSQL gin index
        builder
            .HasIndex(e => e.StringJsonObjects)
            .HasMethod(DbConstant.MethGin);
        // if use builder.OwnsOne, need to create index manually
        builder
            .HasIndex(e => e.ValueJsonObject)
            .HasMethod(DbConstant.MethGin);
        // if use builder.OwnsMany, need to create index manually
        builder
            .HasIndex(e => e.ValueJsonObjects)
            .HasMethod(DbConstant.MethGin);

        // create index manually
        // migrationBuilder.Sql("CREATE INDEX ix_apptable_stringjsonobjects ON app_table USING gin (string_json_objects);");
        // migrationBuilder.Sql("CREATE INDEX ix_apptable_valuejsonobject ON app_table USING gin (value_json_object);");
        // migrationBuilder.Sql("CREATE INDEX ix_apptable_valuejsonobjects ON app_table USING gin (value_json_objects);");

    }
}
