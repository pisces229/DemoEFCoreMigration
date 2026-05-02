using Dapper;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Common.DapperUtil;

public static partial class SqlStatementService
{
    [GeneratedRegex("(?<!^)(?=[A-Z])")]
    private static partial Regex PascalToSnakeRegex();

    private static readonly ConcurrentDictionary<Type, string> _tableNameCache = new();

    /// <summary>
    /// Converts PascalCase class name to snake_case table name for databases using snake_case naming convention.
    /// </summary>
    private static string GetTableName<T>() where T : class =>
        _tableNameCache.GetOrAdd(typeof(T), t =>
            PascalToSnakeRegex().Replace(t.Name, "_").ToLowerInvariant());

    public static string CreateSelect<T>(string where) where T : class
    {
        return $"SELECT * FROM {GetTableName<T>()} {where}";
    }
    public static string CreateInsert<T>(DynamicParameters insertParameters) where T : class
    {
        var insert = new List<string>();
        var values = new List<string>();
        foreach (var name in insertParameters.ParameterNames)
        {
            insert.Add(name);
            values.Add($"@{name}");
        }
        return $"INSERT INTO {GetTableName<T>()} ({string.Join(",", insert)}) VALUES ({string.Join(",", values)})";
    }
    public static string CreateUpdate<T>(DynamicParameters setParameters, string where) where T : class
    {
        if (string.IsNullOrEmpty(where))
        {
            throw new ArgumentException("WHERE clause cannot be empty for UPDATE.", nameof(where));
        }
        var set = new List<string>();
        foreach (var name in setParameters.ParameterNames.Where(w => !w.StartsWith('_')))
        {
            set.Add($"{name} = @{name}");
        }
        return $"UPDATE {GetTableName<T>()} SET {string.Join(",", set)} {where} ";
    }
    public static string CreateDelete<T>(string where) where T : class
    {
        if (string.IsNullOrEmpty(where))
        {
            throw new ArgumentException("WHERE clause cannot be empty for DELETE.", nameof(where));
        }
        return $"DELETE FROM {GetTableName<T>()} {where} ";
    }
}
