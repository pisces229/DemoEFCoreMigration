using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;
using Model.RelEntities;

namespace Model.EntityTypeConfigurations;

internal class LinkFirstContentConfiguration : IEntityTypeConfiguration<LinkFirstContent>
{
    public void Configure(EntityTypeBuilder<LinkFirstContent> builder)
    {
        builder.ToTable(t => t.HasComment("LinkFirstContent"));

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(p => p.LinkFirstSubContents)
            .WithMany()
            .UsingEntity<Dictionary<long, long>>(
                DbContextUtil.ToSnakeCase(nameof(RelLinkFirstSubContent)),
                e => e.HasOne<LinkFirstSubContent>()
                    .WithMany()
                    .HasForeignKey(DbContextUtil.ToSnakeCase(nameof(RelLinkFirstSubContent.LinkSubContentId)))
                    .OnDelete(DeleteBehavior.Cascade),
                e => e.HasOne<LinkFirstContent>()
                    .WithMany()
                    .HasForeignKey(DbContextUtil.ToSnakeCase(nameof(RelLinkFirstSubContent.LinkFirstContentId))),
                e => e.HasKey(
                    DbContextUtil.ToSnakeCase(nameof(RelLinkFirstSubContent.LinkFirstContentId)),
                    DbContextUtil.ToSnakeCase(nameof(RelLinkFirstSubContent.LinkSubContentId)))
            );
    }
}
