namespace IntegrationTest;

/// <summary>
/// PostgreSQL function 整合呼叫驗證:scalar / table-valued、無參與帶參、
/// LINQ-translatable 用法與直接 raw SQL <c>SqlQueryRaw</c>(避開 EF Core 10 對
/// 無 schema + 帶參 TVF 的 NRE bug)。
/// </summary>
[TestClass]
public class TestDbFunc : BaseTest
{
    [TestMethod]
    public Task FuncScalar()
    {
        var funcSaclarResult = _dbContext.EmptyDbSet
            .EmptySqlRaw()
            .Select(s => _dbContext.FuncScalar())
            .FirstOrDefault();

        var saclarValue = _dbContext.Database
            .SqlQuery<int>($"SELECT func_scalar() as \"Value\"")
            .First();

        return Task.CompletedTask;
    }

    [TestMethod]
    public async Task FuncScalarSelect()
    {
        await SeedData.SeedAsync(_dbContext);
        await _dbContext.AnimalCat.Select(s => new
        {
            Id = s.Id,
            Value = _dbContext.FuncScalar()
        })
        .ToListAsync();
    }

    [TestMethod]
    public Task FuncScalarWithParam()
    {
        var funcSaclarResult = _dbContext.EmptyDbSet
            .FromSqlRaw("SELECT 1")
            .Select(s => _dbContext.FuncScalarWithParam(10))
            .FirstOrDefault();

        var saclarValue = _dbContext.Database
            .SqlQuery<int>($"SELECT func_scalar_with_param(10) as \"Value\"")
            .First();

        return Task.CompletedTask;
    }

    [TestMethod]
    public async Task FuncScalarWithParamSelect()
    {
        await SeedData.SeedAsync(_dbContext);
        await _dbContext.AnimalCat.Select(s => new
        {
            Id = s.Id,
            Value = _dbContext.FuncScalarWithParam(10)
        })
        .ToListAsync();
    }

    [TestMethod]
    public async Task FuncTable()
    {
        await _dbContext.FuncTable().ToListAsync();
    }

    [TestMethod]
    public async Task FuncTableInclude()
    {
        await SeedData.SeedAsync(_dbContext);
        await _dbContext.AnimalCat.Include(e => e.FuncTableResult).ToListAsync();
    }

    [TestMethod]
    public async Task FuncTableSelectMany()
    {
        await SeedData.SeedAsync(_dbContext);
        await _dbContext.AnimalCat
            .SelectMany(
                e => _dbContext.FuncTable(),
                (animalCat, funcParamResult) => new { animalCat, funcParamResult }
            )
            .Select(e => new { e.animalCat.Id, e.funcParamResult.Name })
            .ToListAsync();
    }


    [TestMethod]
    public async Task FuncTableWithParam()
    {
        var id = Guid.CreateVersion7();
        await _dbContext.Database
            .SqlQueryRaw<FuncTableWithParamRow>("SELECT id AS \"Id\", name AS \"Name\" FROM func_table_with_param({0})", id)
            .ToListAsync();
    }

    [TestMethod]
    public async Task FuncTableWithParamSelectMany()
    {
        await SeedData.SeedAsync(_dbContext);
        await _dbContext.Database
            .SqlQueryRaw<FuncTableWithParamRow>(
                "SELECT a.id AS \"Id\", f.name AS \"Name\" FROM animal_cat a, LATERAL func_table_with_param(a.id) f")
            .ToListAsync();
    }

    private record FuncTableWithParamRow(Guid Id, string Name);
}
