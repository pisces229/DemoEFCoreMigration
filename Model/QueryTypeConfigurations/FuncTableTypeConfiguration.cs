using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Queries;
using Model.Scripts;

namespace Model.QueryTypeConfigurations;

public class FuncTableTypeConfiguration : IEntityTypeConfiguration<FuncTableResult>
{
    public void Configure(EntityTypeBuilder<FuncTableResult> builder)
    {
        builder.ToFunction(FuncTable.Name);

        builder.HasKey(v => v.Id);
    }
}
