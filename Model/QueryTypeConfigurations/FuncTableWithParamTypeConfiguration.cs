using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Queries;
using Model.Scripts;

namespace Model.QueryTypeConfigurations;

public class FuncTableWithParamTypeConfiguration : IEntityTypeConfiguration<FuncTableWithParamResult>
{
    public void Configure(EntityTypeBuilder<FuncTableWithParamResult> builder)
    {
        builder.ToFunction(FuncTableWithParam.Name);

        builder.HasKey(v => v.Id);
    }
}
