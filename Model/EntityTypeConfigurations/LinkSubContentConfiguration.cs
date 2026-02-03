using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Definitions;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class LinkSubContentConfiguration : IEntityTypeConfiguration<LinkSubContent>
{
    public void Configure(EntityTypeBuilder<LinkSubContent> builder)
    {
        builder.ToTable(e => e.HasComment("LinkSubContent"));

        builder.HasKey(e => e.Id);

        builder.HasDiscriminator(e => e.LinkType)
            .HasValue<LinkFirstSubContent>(LinkSubContentLinkType.First)
            .HasValue<LinkSecondSubContent>(LinkSubContentLinkType.Second);

        builder.Property(e => e.Content)
            .IsRequired()
            .HasMaxLength(100);
    }
}
