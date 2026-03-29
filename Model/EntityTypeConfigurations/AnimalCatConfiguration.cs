namespace Model.EntityTypeConfigurations;

internal class AnimalCatConfiguration : IEntityTypeConfiguration<AnimalCat>
{
    public void Configure(EntityTypeBuilder<AnimalCat> builder)
    {
        builder.ToTable(t => t.HasComment("AnimalCat"));

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Flag)
            .HasDefaultValue(Flag.None);

        builder.Property(c => c.WhiskerLength)
            .HasComment("鬍鬚長度");

        builder.Property(c => c.LovesBox)
            .HasDefaultValue(true);

        builder
            .HasOne(o => o.ViewResult)
            .WithOne()
            .HasForeignKey<ViewResult>(e => e.Id);

        builder
            .HasOne(o => o.FuncTableResult)
            .WithOne()
            .HasForeignKey<FuncTableResult>(e => e.Id);
    }
}
