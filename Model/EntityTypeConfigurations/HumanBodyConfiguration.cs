using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class HumanBodyConfiguration : IEntityTypeConfiguration<HumanBody>
{
    public void Configure(EntityTypeBuilder<HumanBody> builder)
    {
        builder.ToTable(t => t.HasComment("HumanBody"));

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

        builder.HasMany(e => e.HumanLimbs)
            .WithOne(e => e.HumanBody)
            .HasPrincipalKey(e => e.Ulid)
            .HasForeignKey(e => e.BodyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
