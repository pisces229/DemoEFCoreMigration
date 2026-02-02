using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class HumanLimbConfiguration : IEntityTypeConfiguration<HumanLimb>
{
    public void Configure(EntityTypeBuilder<HumanLimb> builder)
    {
        builder.ToTable(t => t.HasComment("HumanLimb"));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Ulid)
            .HasMaxLength(10)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(e => e.Weight)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(e => e.Color).IsRequired();

        builder.Property(e => e.CheckDate)
            //.HasColumnType("DateTime")
            .IsRequired();

        builder.Property(e => e.Remark);
    }
}
