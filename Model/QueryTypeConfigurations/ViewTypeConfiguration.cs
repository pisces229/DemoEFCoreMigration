using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Queries;

namespace Model.QueryTypeConfigurations;

public class ViewTypeConfiguration : IEntityTypeConfiguration<ViewResult>
{
    public void Configure(EntityTypeBuilder<ViewResult> builder)
    {
        builder.ToView(Scripts.View.Name);

        builder.HasKey(v => v.Id);

        builder.Property(e => e.Id)
            .HasColumnName(DbContextUtil.ToSnakeCase(nameof(ViewResult.Id)));
        builder.Property(e => e.Name)
            .HasColumnName(DbContextUtil.ToSnakeCase(nameof(ViewResult.Name)));

    }
}
