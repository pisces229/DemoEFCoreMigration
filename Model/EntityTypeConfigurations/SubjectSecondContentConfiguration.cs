using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

public class SubjectSecondContentConfiguration : IEntityTypeConfiguration<SubjectSecondContent>
{
    public void Configure(EntityTypeBuilder<SubjectSecondContent> builder)
    {
        builder.HasOne(e => e.Subject)
            .WithMany(e => e.Contents)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.ClientCascade)
            .HasConstraintName(SubjectSecondContentForeignKey.ParentId);
    }
}
