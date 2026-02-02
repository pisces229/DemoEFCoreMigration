using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class HumanHeadConfiguration : IEntityTypeConfiguration<HumanHead>
{
    public void Configure(EntityTypeBuilder<HumanHead> builder)
    {
        builder.ToTable(t => t.HasComment("HumanHead"));

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

        builder.HasOne(e => e.HumanBody)
            .WithOne(e => e.HumanHead)
            .HasPrincipalKey<HumanHead>(e => e.Ulid)
            .HasForeignKey<HumanBody>(e => e.HeadId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
