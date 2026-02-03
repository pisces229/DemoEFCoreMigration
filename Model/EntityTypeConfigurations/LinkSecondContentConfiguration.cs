using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;
using Model.RelEntities;

namespace Model.EntityTypeConfigurations;

internal class LinkSecondContentConfiguration : IEntityTypeConfiguration<LinkSecondContent>
{
    public void Configure(EntityTypeBuilder<LinkSecondContent> builder)
    {
        builder.ToTable(e => e.HasComment("LinkSecondContent"));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(p => p.LinkSecondSubContents)
            .WithMany()
            .UsingEntity<Dictionary<long, long>>(
                DbContextUtil.ToSnakeCase(nameof(RelLinkSecondSubContent)),
                e => e.HasOne<LinkSecondSubContent>()
                    .WithMany()
                    .HasForeignKey(DbContextUtil.ToSnakeCase(nameof(RelLinkSecondSubContent.LinkSubContentId)))
                    .OnDelete(DeleteBehavior.Cascade),
                e => e.HasOne<LinkSecondContent>()
                    .WithMany()
                    .HasForeignKey(DbContextUtil.ToSnakeCase(nameof(RelLinkSecondSubContent.LinkSecondContentId))),
                e => e.HasKey(
                    DbContextUtil.ToSnakeCase(nameof(RelLinkSecondSubContent.LinkSecondContentId)),
                    DbContextUtil.ToSnakeCase(nameof(RelLinkSecondSubContent.LinkSubContentId)))
            );
    }
}
