using Microsoft.EntityFrameworkCore;
using Model;

namespace Test;

[TestClass]
public class TestDbFunc : BaseTest
{
    [TestMethod(DisplayName = "FuncScalar")]
    public async Task FuncScalar()
    {
        //await Task.Delay(1);
        //_dbContext.FuncScalar();

        var funcSaclarResult = _dbContext.EmptyDbSet
            .EmptySqlRaw()
            .Select(s => _dbContext.FuncScalar())
            .FirstOrDefault();

        var saclarValue = _dbContext.Database
            .SqlQuery<int>($"SELECT func_scalar() as \"Value\"")
            .First();

        await Task.Delay(1);
    }

    [TestMethod(DisplayName = "FuncScalarSelect")]
    public async Task FuncScalarSelect()
    {
        await _dbContext.AnimalCat.Select(s => new
        {
            Id = s.Id,
            Value = _dbContext.FuncScalar()
        })
        .ToListAsync();
    }

    [TestMethod(DisplayName = "FuncScalarWithParam")]
    public async Task FuncScalarWithParam()
    {
        //await Task.Delay(1);
        //_dbContext.FuncScalarWithParam(10);

        var funcSaclarResult = _dbContext.EmptyDbSet
            .FromSqlRaw("SELECT 1")
            .Select(s => _dbContext.FuncScalarWithParam(10))
            .FirstOrDefault();

        var saclarValue = _dbContext.Database
            .SqlQuery<int>($"SELECT func_scalar_with_param(10) as \"Value\"")
            .First();

        await Task.Delay(1);
    }

    [TestMethod(DisplayName = "FuncScalarWithParamSelect")]
    public async Task FuncScalarWithParamSelect()
    {
        await _dbContext.AnimalCat.Select(s => new
        {
            Id = s.Id,
            Value = _dbContext.FuncScalarWithParam(s.Id)
        })
        .ToListAsync();
    }

    [TestMethod(DisplayName = "FuncTable")]
    public async Task FuncTable()
    {
        await _dbContext.FuncTable().ToListAsync();
    }

    [TestMethod(DisplayName = "FuncTableInclude")]
    public async Task FuncTableInclude()
    {
        await _dbContext.AnimalCat.Include(e => e.FuncTableResult).ToListAsync();
    }

    [TestMethod(DisplayName = "FuncTableSelectMany")]
    public async Task FuncTableSelectMany()
    {
        await _dbContext.AnimalCat
            .SelectMany(
                e => _dbContext.FuncTable(),
                (animalCat, funcParamResult) => new { animalCat, funcParamResult }
            )
            .Select(e => new { e.animalCat.Id, e.funcParamResult.Name })
            .ToListAsync();
    }


    [TestMethod(DisplayName = "FuncTableWithParam")]
    public async Task FuncTableWithParam()
    {
        await _dbContext.FuncTableWithParam(10).ToListAsync();
    }

    [TestMethod(DisplayName = "FuncTableWithParamSelectMany")]
    public async Task FuncTableWithParamSelectMany()
    {
        await _dbContext.AnimalCat
            .SelectMany(
                e => _dbContext.FuncTableWithParam(e.Id),
                (animalCat, funcParamResult) => new { animalCat, funcParamResult }
            )
            .Select(e => new { e.animalCat.Id, e.funcParamResult.Name })
            .ToListAsync();
    }
}
