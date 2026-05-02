using Common.DapperUtil;
using Dapper;
using System.Data;
using System.Text;

using Microsoft.Extensions.Logging;
namespace IntegrationTest;

/// <summary>
/// 透過 <see cref="SqlStatementService"/> 與 <see cref="SqlConditionUtility"/>
/// 動態組裝 SELECT/INSERT/UPDATE/DELETE 並以 Dapper 對 PostgreSQL 執行的整合驗證。
/// 注意:utility 不做 PascalCase → snake_case 轉換,呼叫端需自行給 snake_case column 名。
/// </summary>
[TestClass]
public class TestDapperUtil : BaseTest
{
    [TestMethod]
    public async Task DoDapperSqlStatement()
    {
        await using var connection = (Npgsql.NpgsqlConnection)_dbContext.Database.GetDbConnection();
        await connection.OpenAsync();

        // SELECT
        {
            var condition = new StringBuilder();
            var dynamicParameters = new DynamicParameters();
            SqlConditionUtility.Add(condition, dynamicParameters, "id", SqlOperatorType.Equal, Guid.CreateVersion7(), DbType.Guid);
            var sql = SqlStatementService.CreateSelect<AnimalCat>(condition.ToString());
            _logger.LogInformation("{Message}", SqlStatementUtility.GetSqlString(sql));
            _logger.LogInformation("{Message}", SqlStatementUtility.GetDynamicParameters(dynamicParameters));
            await connection.ExecuteAsync(sql, dynamicParameters);
        }
        // INSERT
        var insertedId = Guid.CreateVersion7();
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("id", insertedId, DbType.Guid);
            dynamicParameters.Add("name", $"Dapper-{Guid.CreateVersion7():N}");
            dynamicParameters.Add("birth_date", DateTime.UtcNow, DbType.DateTime);
            dynamicParameters.Add("whisker_length", 5.5, DbType.Double);
            dynamicParameters.Add("loves_box", true, DbType.Boolean);
            dynamicParameters.Add("flag", 0, DbType.Int32);
            var sql = SqlStatementService.CreateInsert<AnimalCat>(dynamicParameters);
            _logger.LogInformation("{Message}", SqlStatementUtility.GetSqlString(sql));
            _logger.LogInformation("{Message}", SqlStatementUtility.GetDynamicParameters(dynamicParameters));
            var affected = await connection.ExecuteAsync(sql, dynamicParameters);
            Assert.AreEqual(1, affected);
        }
        // UPDATE
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("name", $"Dapper-Updated-{Guid.CreateVersion7():N}");
            dynamicParameters.Add("whisker_length", 7.0, DbType.Double);
            var condition = new StringBuilder();
            SqlConditionUtility.Add(condition, dynamicParameters, "id", SqlOperatorType.Equal, insertedId, DbType.Guid);
            var sql = SqlStatementService.CreateUpdate<AnimalCat>(dynamicParameters, condition.ToString());
            _logger.LogInformation("{Message}", SqlStatementUtility.GetSqlString(sql));
            _logger.LogInformation("{Message}", SqlStatementUtility.GetDynamicParameters(dynamicParameters));
            var affected = await connection.ExecuteAsync(sql, dynamicParameters);
            Assert.AreEqual(1, affected);
        }
        // DELETE
        {
            var dynamicParameters = new DynamicParameters();
            var condition = new StringBuilder();
            SqlConditionUtility.Add(condition, dynamicParameters, "id", SqlOperatorType.Equal, insertedId, DbType.Guid);
            var sql = SqlStatementService.CreateDelete<AnimalCat>(condition.ToString());
            _logger.LogInformation("{Message}", SqlStatementUtility.GetSqlString(sql));
            _logger.LogInformation("{Message}", SqlStatementUtility.GetDynamicParameters(dynamicParameters));
            var affected = await connection.ExecuteAsync(sql, dynamicParameters);
            Assert.AreEqual(1, affected);
        }
    }

    [TestMethod]
    public void CreateUpdateThrowsOnEmptyWhere()
    {
        var setParameters = new DynamicParameters();
        setParameters.Add("name", "x");
        Assert.ThrowsExactly<ArgumentException>(
            () => SqlStatementService.CreateUpdate<AnimalCat>(setParameters, string.Empty));
    }

    [TestMethod]
    public void CreateDeleteThrowsOnEmptyWhere()
    {
        Assert.ThrowsExactly<ArgumentException>(
            () => SqlStatementService.CreateDelete<AnimalCat>(string.Empty));
    }
}
