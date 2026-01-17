using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Queries;

namespace Model.QueryTypeConfigurations;

public class KeyLessEntityTypeConfiguration : IEntityTypeConfiguration<KeyLessResult>
{
    public void Configure(EntityTypeBuilder<KeyLessResult> builder)
    {
        builder.ToTable(t => t.ExcludeFromMigrations());

        builder.HasNoKey();
    }
}
