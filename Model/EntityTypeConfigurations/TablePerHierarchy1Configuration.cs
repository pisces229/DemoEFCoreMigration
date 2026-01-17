using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class TablePerHierarchy1Configuration : IEntityTypeConfiguration<TablePerHierarchy1>
{
    public void Configure(EntityTypeBuilder<TablePerHierarchy1> builder)
    {
        builder.Property(e => e.Name1)
            .HasMaxLength(10)
            .IsUnicode(true);
    }
}
