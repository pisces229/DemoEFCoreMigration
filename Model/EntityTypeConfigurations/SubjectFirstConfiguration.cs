namespace Model.EntityTypeConfigurations;

public class SubjectFirstConfiguration : IEntityTypeConfiguration<SubjectFirst>
{
    public void Configure(EntityTypeBuilder<SubjectFirst> builder)
    {
        builder.ToTable(t => t.HasComment("SubjectFirst"));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
    }
}
