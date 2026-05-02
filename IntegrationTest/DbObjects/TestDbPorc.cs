using Npgsql;

namespace IntegrationTest;

/// <summary>
/// PostgreSQL stored procedure 呼叫驗證:無參的 <c>proc()</c> 與帶
/// IN / INOUT 參數的 overload(<c>proc(INT, INOUT TEXT)</c>),透過
/// <c>ExecuteSqlInterpolated</c> / <c>ExecuteSqlRaw</c> 與 <see cref="NpgsqlParameter"/> 雙向傳遞。
/// </summary>
[TestClass]
public class TestDbPorc : BaseTest
{
    [TestMethod]
    public async Task ExecuteSqlInterpolated()
    {
        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"CALL proc()");
    }

    [TestMethod]
    public async Task CallWithParam()
    {
        var idParam = new NpgsqlParameter("p_id", 101);
        var nameParam = new NpgsqlParameter("p_name", NpgsqlTypes.NpgsqlDbType.Text)
        {
            Direction = System.Data.ParameterDirection.InputOutput,
            Value = "",
        };
        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL proc(@p_id, @p_name)",
            idParam, nameParam);

        Assert.AreEqual("id-101", nameParam.Value);
    }
}
