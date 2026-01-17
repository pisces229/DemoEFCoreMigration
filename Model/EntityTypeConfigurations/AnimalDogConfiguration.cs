using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Definitions;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class AnimalDogConfiguration : IEntityTypeConfiguration<AnimalDog>
{
    public void Configure(EntityTypeBuilder<AnimalDog> builder)
    {
        builder.ToTable(t => t.HasComment("AnimalDog"));

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Flag)
            .HasDefaultValue(Flag.None);

        builder.Property(d => d.Breed)
            .HasMaxLength(50);

        builder.Property(d => d.IsGoodBoy)
            .HasDefaultValue(true);
    }
}
