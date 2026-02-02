using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Definitions;
using Model.Entities;
using Model.Names;

namespace Model.EntityTypeConfigurations;

public class SubjectFirstContentConfiguration : IEntityTypeConfiguration<SubjectFirstContent>
{
    public void Configure(EntityTypeBuilder<SubjectFirstContent> builder)
    {
        builder.HasOne(e => e.Subject)
            .WithMany(e => e.Contents)
            .HasForeignKey(e => e.ReferenceId)
            .OnDelete(DeleteBehavior.ClientCascade)
            .HasConstraintName(SubjectContentConstraintName.Discriminators[SubjectContentReferenceType.First]);
    }
}
