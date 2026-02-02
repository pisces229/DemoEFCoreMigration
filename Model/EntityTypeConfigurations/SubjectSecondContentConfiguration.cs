using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Definitions;
using Model.Entities;
using Model.Names;

namespace Model.EntityTypeConfigurations;

public class SubjectSecondContentConfiguration : IEntityTypeConfiguration<SubjectSecondContent>
{
    public void Configure(EntityTypeBuilder<SubjectSecondContent> builder)
    {
        builder.HasOne(e => e.Subject)
            .WithMany(e => e.Contents)
            .HasForeignKey(e => e.ReferenceId)
            .OnDelete(DeleteBehavior.ClientCascade)
            .HasConstraintName(SubjectContentConstraintName.Discriminators[SubjectContentReferenceType.Second]);
    }
}
