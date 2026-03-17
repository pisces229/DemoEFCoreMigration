using Model.IEntities;

namespace Model.Extensions;

public static class EntityTypeBuilderExtensions
{

    public static EntityTypeBuilder<TEntity> ConfigureFamilyChildEntite<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IFamilyChildEntite
    {
        builder.Property(e => e.ParentId)
            .HasColumnName("parent_id")
            .IsRequired();

        // Defining HasOne relationships here is not recommended:
        // 1. Avoids creating redundant Shadow Properties (e.g., FamilyParentId) which cause duplicate columns.
        // 2. Ensures Navigation Properties in entity classes map correctly in their specific configurations.
        // builder.HasOne<FamilyParent>()
        //     .WithMany()
        //     .HasForeignKey(e => e.ParentId)
        //     .IsRequired();

        return builder;
    }

    //public static EntityTypeBuilder ConfigureFamilyChildEntite(this EntityTypeBuilder builder)
    //{
    //    builder.Property(nameof(IFamilyChildEntite.ParentId))
    //        .IsRequired();

    //    return builder;
    //}

    public static EntityTypeBuilder<TEntity> ConfigureCreateEntite<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, ICreateEntite
    {
        builder.Property(e => e.CreatedAt)
            .HasComment("created_at")
            .IsRequired()
            .HasColumnType(DbColumnType.TimestampWithTimeZone);

        return builder;
    }

    //public static EntityTypeBuilder ConfigureCreateEntite(this EntityTypeBuilder builder)
    //{
    //    builder.Property(nameof(ICreateEntite.CreatedAt))
    //        .IsRequired()
    //        .HasColumnType(DbColumnType.TimestampWithTimeZone);

    //    return builder;
    //}

    public static EntityTypeBuilder<TEntity> ConfigureUpdateEntite<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IUpdateEntite
    {
        builder.Property(e => e.UpdatedAt)
            .HasComment("updated_at")
            .IsRequired()
            .HasColumnType(DbColumnType.TimestampWithTimeZone);

        return builder;
    }

    //public static EntityTypeBuilder ConfigureUpdateEntite(this EntityTypeBuilder builder)
    //{
    //    builder.Property(nameof(IUpdateEntite.UpdatedAt))
    //        .IsRequired()
    //        .HasColumnType(DbColumnType.TimestampWithTimeZone);

    //    return builder;
    //}
}
