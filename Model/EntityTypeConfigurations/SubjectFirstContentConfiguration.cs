using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class SubjectFirstContentConfiguration : IEntityTypeConfiguration<SubjectFirstContent>
{
    public void Configure(EntityTypeBuilder<SubjectFirstContent> builder)
    {
        builder.HasOne(e => e.Subject)
            .WithMany(e => e.Contents)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.ClientCascade)
            .HasConstraintName(SubjectFirstContentForeignKey.ParentId);
    }
}
