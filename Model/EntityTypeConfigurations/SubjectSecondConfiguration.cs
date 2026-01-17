using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class SubjectSecondConfiguration : IEntityTypeConfiguration<SubjectSecond>
{
    public void Configure(EntityTypeBuilder<SubjectSecond> builder)
    {
        builder.ToTable(t => t.HasComment("SubjectSecond"));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
    }
}
