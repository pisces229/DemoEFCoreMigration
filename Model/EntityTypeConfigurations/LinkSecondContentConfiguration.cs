using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

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
            .UsingEntity<Dictionary<string, string>>(
                "rel_second_sub_content",
                e => e.HasOne<LinkSecondSubContent>()
                      .WithMany()
                      .HasForeignKey("link_sub_content_id")
                      .OnDelete(DeleteBehavior.Cascade),
                e => e.HasOne<LinkSecondContent>()
                      .WithMany()
                      .HasForeignKey("link_second_content_id"),
                e => e.HasKey("link_second_content_id", "link_sub_content_id")
            );
    }
}
