namespace Model.EntityTypeConfigurations;

public class LinkSubContentConfiguration : IEntityTypeConfiguration<LinkSubContent>
{
    public void Configure(EntityTypeBuilder<LinkSubContent> builder)
    {
        builder.ToTable(e => e.HasComment("LinkSubContent"));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.HasDiscriminator(e => e.LinkType)
            .HasValue<LinkFirstSubContent>(LinkSubContentLinkType.First)
            .HasValue<LinkSecondSubContent>(LinkSubContentLinkType.Second);

        builder.Property(e => e.Content)
            .IsRequired()
            .HasMaxLength(100);
    }
}
