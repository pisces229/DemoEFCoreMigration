using Dapper;
using System.Data;
using System.Text;

namespace Common.DapperUtil;

public static class SqlConditionUtility
{
    /// <summary>
    /// WARNING: This overload directly concatenates the condition string without parameterization.
    /// Only use this with compile-time constants or fully validated input to avoid SQL injection.
    /// Prefer the parameterized Add overloads instead.
    /// </summary>
    public static void Add(StringBuilder sql, string condition)
        => sql.Append($"{StartWithWhereOrAnd(sql)} {condition} ");
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
        if (value == null) return;
        if (value is string str && string.IsNullOrEmpty(str)) return;

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
                sql.Append($"{StartWithWhereOrAnd(sql)} {condition} {GetOperator(sqlOperator)} {CreateParameter(dynamicParameters, sqlOperator, value, dbType)} ");
                break;
            default:
                throw new NotSupportedException($"SqlOperator '{sqlOperator}' is not supported for scalar values.");
        }
    }
    public static void Add<T>(StringBuilder sql, DynamicParameters dynamicParameters,
        string condition, SqlOperatorType sqlOperator, IEnumerable<T> value)
    {
        if (value == null || !value.Any()) return;

        switch (sqlOperator)
        {
            case SqlOperatorType.Contain:
            case SqlOperatorType.NotContain:
                sql.Append($"{StartWithWhereOrAnd(sql)} {condition} {GetOperator(sqlOperator)} {CreateParameter(dynamicParameters, sqlOperator, value, DbType.Object)} ");
                break;
            default:
                throw new NotSupportedException($"SqlOperator '{sqlOperator}' is not supported for collection values.");
        }
    }
    /// <summary>
    /// Determines whether to append WHERE or AND based on whether the SQL already contains a WHERE clause.
    /// This logic assumes the StringBuilder only contains the WHERE clause portion (no ORDER BY, LIMIT, etc).
    /// </summary>
    private static string StartWithWhereOrAnd(StringBuilder sql) => sql.Length == 0 ? " WHERE" : " AND";
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
        var parameterName = $"__p{dynamicParameters.ParameterNames.Count(p => p.StartsWith("__p"))}";
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
