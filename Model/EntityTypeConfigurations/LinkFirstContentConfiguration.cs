using Model.RelEntities;

namespace Model.EntityTypeConfigurations;

internal class LinkFirstContentConfiguration : IEntityTypeConfiguration<LinkFirstContent>
{
    public void Configure(EntityTypeBuilder<LinkFirstContent> builder)
    {
        builder.ToTable(t => t.HasComment("LinkFirstContent"));

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(p => p.LinkFirstSubContents)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                DbContextUtil.NamingConvention(nameof(RelLinkFirstSubContent)),
                e => e.HasOne<LinkFirstSubContent>()
                    .WithMany()
                    .HasForeignKey(DbContextUtil.NamingConvention(nameof(RelLinkFirstSubContent.LinkSubContentId)))
                    .OnDelete(DeleteBehavior.Cascade),
                e => e.HasOne<LinkFirstContent>()
                    .WithMany()
                    .HasForeignKey(DbContextUtil.NamingConvention(nameof(RelLinkFirstSubContent.LinkFirstContentId))),
                e => e.HasKey(
                    DbContextUtil.NamingConvention(nameof(RelLinkFirstSubContent.LinkFirstContentId)),
                    DbContextUtil.NamingConvention(nameof(RelLinkFirstSubContent.LinkSubContentId)))
            );
    }
}
