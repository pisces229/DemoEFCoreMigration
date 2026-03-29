using Model.Scripts;

namespace Model.Extensions;

public static class ModelBuilderExtensions
{
    public static void RegisterFunctions(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasDbFunction(typeof(ApplicationDbContext)
            .GetMethod(nameof(ApplicationDbContext.FuncTable), Type.EmptyTypes)!)
            .HasName(FuncTable.Name)
            .HasSchema(DbContextUtil.Schema);

        modelBuilder.HasDbFunction(typeof(ApplicationDbContext)
            .GetMethod(nameof(ApplicationDbContext.FuncTableWithParam), [typeof(Guid)])!)
            .HasName(FuncTableWithParam.Name)
            .HasSchema(DbContextUtil.Schema);

        modelBuilder.HasDbFunction(typeof(ApplicationDbContext)
            .GetMethod(nameof(ApplicationDbContext.FuncScalar), Type.EmptyTypes)!)
            .HasName(FuncScalar.Name)
            .HasSchema(DbContextUtil.Schema);

        modelBuilder.HasDbFunction(typeof(ApplicationDbContext)
            .GetMethod(nameof(ApplicationDbContext.FuncScalarWithParam), [typeof(long)])!)
            .HasName(FuncScalarWithParam.Name)
            .HasSchema(DbContextUtil.Schema);
    }
}
