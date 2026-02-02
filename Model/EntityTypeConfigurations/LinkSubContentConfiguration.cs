using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class LinkSubContentConfiguration : IEntityTypeConfiguration<LinkSubContent>
{
    public void Configure(EntityTypeBuilder<LinkSubContent> builder)
    {
        builder.ToTable(e => e.HasComment("LinkSubContent"));

        builder.HasKey(e => e.Id);

        builder
            .HasDiscriminator<string>("link_type")
            .HasValue<LinkFirstSubContent>("first")
            .HasValue<LinkSecondSubContent>("second");

        builder.Property(e => e.Content)
            .IsRequired()
            .HasMaxLength(100);
    }
}
