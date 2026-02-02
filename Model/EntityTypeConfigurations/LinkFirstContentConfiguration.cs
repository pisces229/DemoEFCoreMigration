using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

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
            .UsingEntity<Dictionary<string, string>>(
                "rel_first_sub_content",
                e => e.HasOne<LinkFirstSubContent>()
                      .WithMany()
                      .HasForeignKey("link_sub_content_id")
                      .OnDelete(DeleteBehavior.Cascade),
                e => e.HasOne<LinkFirstContent>()
                      .WithMany()
                      .HasForeignKey("link_first_content_id"),
                e => e.HasKey("link_first_content_id", "link_sub_content_id")
            );
    }
}
