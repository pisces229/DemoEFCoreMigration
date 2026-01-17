using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class TablePerHierarchy2Configuration : IEntityTypeConfiguration<TablePerHierarchy2>
{
    public void Configure(EntityTypeBuilder<TablePerHierarchy2> builder)
    {
        builder.Property(e => e.Name2)
            .HasMaxLength(10)
            .IsUnicode(true);
    }
}
