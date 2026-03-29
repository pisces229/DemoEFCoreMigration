namespace Model.EntityTypeConfigurations;

public class FamilyParentConfiguration : IEntityTypeConfiguration<FamilyParent>
{
    public void Configure(EntityTypeBuilder<FamilyParent> builder)
    {
        builder.ToTable(t => t.HasComment("FamilyParent"));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.ConfigureCreateEntite();
        builder.ConfigureUpdateEntite();

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
