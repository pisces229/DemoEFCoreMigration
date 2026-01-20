using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Definitions;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class SubjectContentConfiguration : IEntityTypeConfiguration<SubjectContent>
{
    public void Configure(EntityTypeBuilder<SubjectContent> builder)
    {
        builder.ToTable(t =>
        {
            //t.HasCheckConstraint("CK_TablePerHierarchyTable_TypeName", $"TypeName IN ('1', '2')");
        });

        builder.HasKey(e => e.Id);

        builder.HasDiscriminator(e => e.ReferenceType)
            .HasValue<SubjectFirstContent>(SubjectContentReferenceType.First)
            .HasValue<SubjectSecondContent>(SubjectContentReferenceType.Second);

        //builder.HasQueryFilter(c => c.Type == SubjectContentType.First || c.Type == SubjectContentType.Second);

        builder.Property(e => e.Content).HasMaxLength(50).IsRequired();

        builder.HasIndex(e => new { e.ReferenceId, e.ReferenceType });
    }
}
