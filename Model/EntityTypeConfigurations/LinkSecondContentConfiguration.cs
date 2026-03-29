using Model.RelEntities;

namespace Model.EntityTypeConfigurations;

internal class LinkSecondContentConfiguration : IEntityTypeConfiguration<LinkSecondContent>
{
    public void Configure(EntityTypeBuilder<LinkSecondContent> builder)
    {
        builder.ToTable(e => e.HasComment("LinkSecondContent"));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(p => p.LinkSecondSubContents)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                DbContextUtil.NamingConvention(nameof(RelLinkSecondSubContent)),
                e => e.HasOne<LinkSecondSubContent>()
                    .WithMany()
                    .HasForeignKey(DbContextUtil.NamingConvention(nameof(RelLinkSecondSubContent.LinkSubContentId)))
                    .OnDelete(DeleteBehavior.Cascade),
                e => e.HasOne<LinkSecondContent>()
                    .WithMany()
                    .HasForeignKey(DbContextUtil.NamingConvention(nameof(RelLinkSecondSubContent.LinkSecondContentId))),
                e => e.HasKey(
                    DbContextUtil.NamingConvention(nameof(RelLinkSecondSubContent.LinkSecondContentId)),
                    DbContextUtil.NamingConvention(nameof(RelLinkSecondSubContent.LinkSubContentId)))
            );
    }
}
