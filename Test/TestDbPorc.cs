using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Test;

[TestClass]
public class TestDbPorc : BaseTest
{
    [TestMethod(DisplayName = "ExecuteSqlInterpolated")]
    public async Task ExecuteSqlInterpolated()
    {
        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"CALL proc()");
        //await _dbContext.Database.ExecuteSqlInterpolatedAsync($"CALL proc({0})");
    }

    [Ignore]
    [TestMethod(DisplayName = "CallWithParam")]
    public async Task CallWithParam()
    {
        var idParam = new NpgsqlParameter("p_id", 101);
        var nameParam = new NpgsqlParameter("p_name", NpgsqlTypes.NpgsqlDbType.Text)
        {
            Direction = System.Data.ParameterDirection.InputOutput,
            Value = ""
        };
        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL proc(@p_id, @p_name)",
            idParam, nameParam
        );
        var resultName = nameParam.Value;
    }
}
