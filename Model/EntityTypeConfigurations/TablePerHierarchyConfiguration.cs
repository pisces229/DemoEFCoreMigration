using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class TablePerHierarchyConfiguration : IEntityTypeConfiguration<TablePerHierarchy>
{
    public void Configure(EntityTypeBuilder<TablePerHierarchy> builder)
    {
        builder.ToTable(t =>
        {
            //t.HasCheckConstraint("CK_TablePerHierarchyTable_TypeName", $"TypeName IN ('1', '2')");
        });

        builder.HasKey(e => e.Id);

        builder.HasDiscriminator(e => e.TypeName)
            .HasValue<TablePerHierarchy1>("1")
            .HasValue<TablePerHierarchy2>("2");

        builder.Property(e => e.TypeName)
            .HasMaxLength(50)
            .IsUnicode(true)
            .IsRequired();

        builder.Property(e => e.Name)
            .HasMaxLength(10)
            .IsUnicode(true)
            .IsRequired();

        builder.HasIndex(e => new { e.TypeName });
    }
}
