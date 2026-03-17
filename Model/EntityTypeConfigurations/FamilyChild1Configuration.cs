namespace Model.EntityTypeConfigurations;

public class FamilyChild1Configuration : IEntityTypeConfiguration<FamilyChild1>
{
    public void Configure(EntityTypeBuilder<FamilyChild1> builder)
    {
        builder.ToTable(t => t.HasComment("FamilyChild1"));

        builder.HasKey(e => e.Id);

        builder.ConfigureFamilyChildEntite();
        builder.ConfigureCreateEntite();
        builder.ConfigureUpdateEntite();

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        // Option A: Use Shadow Navigation Property
        // Best for: Keeping the entity class clean (no 'FamilyParent' property in C#).
        // Note: You must use strings for Include, e.g., .Include("FamilyParent")
        //builder.HasOne<FamilyParent>()
        //    .WithMany()
        //    .HasForeignKey(e => e.ParentId)
        //    .IsRequired();

        // Option B: Use Explicit Navigation Property (Recommended)
        // Best for: Strongly-typed queries and easy access, e.g., child.FamilyParent.Name
        // Note: Requires 'FamilyParent' property to exist in the TEntity class.
        builder.HasOne(e => e.FamilyParent)
           .WithMany(p => p.FamilyChild1)
           .HasForeignKey(e => e.ParentId)
           .IsRequired();
    }
}
