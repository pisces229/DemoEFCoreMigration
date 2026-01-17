using Common.DapperUtil;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using System.Text;

namespace Test;

public class TestDapperUtil : BaseTest
{
    public async Task DoDapperSqlStatement()
    {
        using (var connection = _dbContext.Database.GetDbConnection())
        {
            try
            {
                await connection.OpenAsync();
                {
                    var condition = new StringBuilder();
                    var dynamicParameters = new DynamicParameters();
                    SqlConditionUtility.Add(condition, dynamicParameters, $"{nameof(AppTable.Id)}", SqlOperatorType.Equal, "@@@@");
                    var sql = SqlStatementService.CreateSelect<AppTable>(condition.ToString());
                    Console.WriteLine(SqlStatementUtility.GetSqlString(sql));
                    Console.WriteLine(SqlStatementUtility.GetDynamicParameters(dynamicParameters));
                    await connection.ExecuteAsync(sql, dynamicParameters);
                }
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add(nameof(AppTable.Id), Guid.NewGuid().ToString());
                    dynamicParameters.Add(nameof(AppTable.String), Guid.NewGuid().ToString());
                    var sql = SqlStatementService.CreateInsert<AppTable>(dynamicParameters);
                    Console.WriteLine(SqlStatementUtility.GetSqlString(sql));
                    Console.WriteLine(SqlStatementUtility.GetDynamicParameters(dynamicParameters));
                    await connection.ExecuteAsync(sql, dynamicParameters);
                }
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add(nameof(AppTable.Id), Guid.NewGuid().ToString());
                    dynamicParameters.Add(nameof(AppTable.String), Guid.NewGuid().ToString());
                    var condition = new StringBuilder();
                    SqlConditionUtility.Add(condition, dynamicParameters, $"{nameof(AppTable.Id)}", SqlOperatorType.Equal, "@@@@");
                    var sql = SqlStatementService.CreateUpdate<AppTable>(dynamicParameters, condition.ToString());
                    Console.WriteLine(SqlStatementUtility.GetSqlString(sql));
                    Console.WriteLine(SqlStatementUtility.GetDynamicParameters(dynamicParameters));
                    await connection.ExecuteAsync(sql, dynamicParameters);
                }
                {
                    var dynamicParameters = new DynamicParameters();
                    var condition = new StringBuilder();
                    SqlConditionUtility.Add(condition, dynamicParameters, $"{nameof(AppTable.Id)}", SqlOperatorType.Equal, "@@@@");
                    var sql = SqlStatementService.CreateDelete<AppTable>(condition.ToString());
                    Console.WriteLine(SqlStatementUtility.GetSqlString(sql));
                    Console.WriteLine(SqlStatementUtility.GetDynamicParameters(dynamicParameters));
                    await connection.ExecuteAsync(sql, dynamicParameters);
                }
                //using (var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted))
                //{
                //    {
                //        var dynamicParameters = new DynamicParameters();
                //        dynamicParameters.Add(nameof(DemoTable.Id), Guid.NewGuid().ToString());
                //        dynamicParameters.Add(nameof(DemoTable.StringValue), Guid.NewGuid().ToString());
                //        var sql = SqlStatementService.CreateInsert<DemoTable>(dynamicParameters);
                //        Console.WriteLine(SqlStatementUtility.GetSqlString(sql));
                //        Console.WriteLine(SqlStatementUtility.GetDynamicParameters(dynamicParameters));
                //        await connection.ExecuteAsync(sql, dynamicParameters, transaction);
                //    }
                //    {
                //        var dynamicParameters = new DynamicParameters();
                //        dynamicParameters.Add(nameof(DemoTable.Id), Guid.NewGuid().ToString());
                //        dynamicParameters.Add(nameof(DemoTable.StringValue), Guid.NewGuid().ToString());
                //        var sql = SqlStatementService.CreateInsert<DemoTable>(dynamicParameters);
                //        Console.WriteLine(SqlStatementUtility.GetSqlString(sql));
                //        Console.WriteLine(SqlStatementUtility.GetDynamicParameters(dynamicParameters));
                //        await connection.ExecuteAsync(sql, dynamicParameters, transaction);
                //    }
                //    //await transaction.CommitAsync();
                //    //await transaction.RollbackAsync();
                //}
            }
            finally
            {
                try
                {
                    await connection.CloseAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
