namespace Model.EntityTypeConfigurations;

public class ClosurePathConfiguration : IEntityTypeConfiguration<ClosurePath>
{
    public void Configure(EntityTypeBuilder<ClosurePath> builder)
    {
        builder.ToTable(t => t.HasComment("ClosurePath"));

        builder.HasKey(e => new { e.AncestorId, e.DescendantId });

        // Index on DescendantId for efficient ancestor queries (WHERE DescendantId = X)
        builder.HasIndex(e => e.DescendantId);

        builder.HasOne(d => d.Ancestor)
            .WithMany(p => p.DescendantPaths)
            .HasForeignKey(d => d.AncestorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Descendant)
            .WithMany(p => p.AncestorPaths)
            .HasForeignKey(d => d.DescendantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
