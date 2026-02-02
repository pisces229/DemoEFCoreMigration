using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class AppIndexConfiguration : IEntityTypeConfiguration<AppIndex>
{
    public void Configure(EntityTypeBuilder<AppIndex> builder)
    {
        builder.ToTable(t => t.HasComment("AppIndex"));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.C1)
            .IsUnicode(true)
            .HasMaxLength(10)
            .IsRequired()
            .HasDefaultValue("C1");

        builder.Property(e => e.C2)
            .IsUnicode(false)
            .HasMaxLength(20)
            .IsRequired()
            .HasDefaultValue("C2");

        builder.HasIndex(e => e.C1).IsUnique();

        builder.HasIndex(e => new { e.C1, e.C2 });
    }
}
