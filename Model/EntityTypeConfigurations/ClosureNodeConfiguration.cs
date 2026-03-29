namespace Model.EntityTypeConfigurations;

public class ClosureNodeConfiguration : IEntityTypeConfiguration<ClosureNode>
{
    public void Configure(EntityTypeBuilder<ClosureNode> builder)
    {
        builder.ToTable(t => t.HasComment("ClosureNode"));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
