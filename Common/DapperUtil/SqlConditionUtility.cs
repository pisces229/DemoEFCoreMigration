using Dapper;
using System.Data;
using System.Text;

namespace Common.DapperUtil;

public static class SqlConditionUtility
{
    public static void Add(StringBuilder sql, string condition)
        => sql.Append($"{StratWithWhereOrAnd(sql)} {condition} ");
    public static void Add(StringBuilder sql, DynamicParameters dynamicParameters,
        string condition, SqlOperatorType sqlOperator, string? value, DbType dbType = DbType.String)
        => AddValue(sql, dynamicParameters, condition, sqlOperator, value, dbType);
    public static void Add(StringBuilder sql, DynamicParameters dynamicParameters,
        string condition, SqlOperatorType sqlOperator, int? value, DbType dbType = DbType.Int32)
        => AddValue(sql, dynamicParameters, condition, sqlOperator, value, dbType);
    public static void Add(StringBuilder sql, DynamicParameters dynamicParameters,
        string condition, SqlOperatorType sqlOperator, double? value, DbType dbType = DbType.Double)
         => AddValue(sql, dynamicParameters, condition, sqlOperator, value, dbType);
    public static void Add(StringBuilder sql, DynamicParameters dynamicParameters,
        string condition, SqlOperatorType sqlOperator, DateTime? value, DbType dbType = DbType.DateTime)
        => AddValue(sql, dynamicParameters, condition, sqlOperator, value, dbType);
    public static void Add<T>(StringBuilder sql, DynamicParameters dynamicParameters,
        string condition, SqlOperatorType sqlOperator, T value, DbType dbType)
         => AddValue(sql, dynamicParameters, condition, sqlOperator, value, dbType);
    private static void AddValue<T>(StringBuilder sql, DynamicParameters dynamicParameters,
        string condition, SqlOperatorType sqlOperator, T value, DbType dbType)
    {
        if (value == null)
        {
            return;
        }
        switch (sqlOperator)
        {
            case SqlOperatorType.Equal:
            case SqlOperatorType.NotEqual:
            case SqlOperatorType.GreaterThan:
            case SqlOperatorType.GreaterThanOrEqual:
            case SqlOperatorType.LessThan:
            case SqlOperatorType.LessThanOrEqual:
            case SqlOperatorType.LikeStart:
            case SqlOperatorType.NotLikeStart:
            case SqlOperatorType.LikeEnd:
            case SqlOperatorType.NotLikeEnd:
            case SqlOperatorType.LikeContain:
            case SqlOperatorType.NotLikeContain:
                if (value.GetType() == typeof(string))
                {
                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        sql.Append($"{StratWithWhereOrAnd(sql)} {condition} {GetOperator(sqlOperator)} {CreateParameter(dynamicParameters, sqlOperator, value, dbType)} ");
                    }
                }
                else
                {
                    sql.Append($"{StratWithWhereOrAnd(sql)} {condition} {GetOperator(sqlOperator)} {CreateParameter(dynamicParameters, sqlOperator, value, dbType)} ");
                }
                break;
        }
    }
    public static void Add<T>(StringBuilder sql, DynamicParameters dynamicParameters,
        string condition, SqlOperatorType sqlOperator, IEnumerable<T> value)
    {
        if (value == null || !value.Any())
        {
            return;
        }
        switch (sqlOperator)
        {
            case SqlOperatorType.Contain:
            case SqlOperatorType.NotContain:
                sql.Append($"{StratWithWhereOrAnd(sql)} {condition} {GetOperator(sqlOperator)} {CreateParameter(dynamicParameters, sqlOperator, value, DbType.Object)} ");
                break;
        }
    }
    private static string StratWithWhereOrAnd(StringBuilder sql) => sql.Length == 0 ? " WHERE" : " AND";
    private static string GetOperator(SqlOperatorType sqlOperator)
        => sqlOperator switch
        {
            SqlOperatorType.Equal => "=",
            SqlOperatorType.NotEqual => "<>",
            SqlOperatorType.GreaterThan => ">",
            SqlOperatorType.GreaterThanOrEqual => ">=",
            SqlOperatorType.LessThan => "<",
            SqlOperatorType.LessThanOrEqual => "<=",
            SqlOperatorType.LikeStart => "LIKE",
            SqlOperatorType.LikeEnd => "LIKE",
            SqlOperatorType.LikeContain => "LIKE",
            SqlOperatorType.NotLikeStart => "NOT LIKE",
            SqlOperatorType.NotLikeEnd => "NOT LIKE",
            SqlOperatorType.NotLikeContain => "NOT LIKE",
            SqlOperatorType.Contain => "IN",
            SqlOperatorType.NotContain => "NOT IN",
            _ => throw new NotImplementedException("SqlOperator is not exist"),
        };
    private static string CreateParameter<T>(DynamicParameters dynamicParameters, SqlOperatorType sqlOperator, T value, DbType dbType)
    {
        var parameterName = $"__p{dynamicParameters.ParameterNames.Where(p => p.StartsWith("__p")).Count()}";
        switch (sqlOperator)
        {
            case SqlOperatorType.Equal:
            case SqlOperatorType.NotEqual:
            case SqlOperatorType.GreaterThan:
            case SqlOperatorType.GreaterThanOrEqual:
            case SqlOperatorType.LessThan:
            case SqlOperatorType.LessThanOrEqual:
                dynamicParameters.Add(parameterName, value, dbType);
                break;
            case SqlOperatorType.LikeStart:
            case SqlOperatorType.NotLikeStart:
                dynamicParameters.Add(parameterName, $"{value}%", dbType);
                break;
            case SqlOperatorType.LikeEnd:
            case SqlOperatorType.NotLikeEnd:
                dynamicParameters.Add(parameterName, $"%{value}", dbType);
                break;
            case SqlOperatorType.LikeContain:
            case SqlOperatorType.NotLikeContain:
                dynamicParameters.Add(parameterName, $"%{value}%", dbType);
                break;
            case SqlOperatorType.Contain:
            case SqlOperatorType.NotContain:
                dynamicParameters.Add(parameterName, value);
                break;
            default:
                throw new NotImplementedException();
        }
        return $"@{parameterName}";
    }
}
