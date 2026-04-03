using Dapper;
using System.Text.RegularExpressions;

namespace Common.DapperUtil;

public static class SqlStatementService
{
    /// <summary>
    /// Converts PascalCase class name to snake_case table name for databases using snake_case naming convention.
    /// </summary>
    private static string GetTableName<T>() where T : class
    {
        var typeName = typeof(T).Name;
        // Convert PascalCase to snake_case: "AppTable" -> "app_table"
        return Regex.Replace(typeName, "(?<!^)(?=[A-Z])", "_").ToLowerInvariant();
    }

    public static string CreateSelect<T>(string where) where T : class
    {
        return $"SELECT * FROM {GetTableName<T>()} {where}";
    }
    public static string CreateInsert<T>(DynamicParameters insertParameters) where T : class
    {
        var insert = new List<string>();
        var values = new List<string>();
        insertParameters.ParameterNames.ToList().ForEach(f =>
        {
            insert.Add(f);
            values.Add($"@{f}");
        });
        return $"INSERT INTO {GetTableName<T>()} ({string.Join(",", insert)}) VALUES ({string.Join(",", values)})";
    }
    public static string CreateUpdate<T>(DynamicParameters setParameters, string where) where T : class
    {
        if (!string.IsNullOrEmpty(where))
        {
            var set = new List<string>();
            setParameters.ParameterNames.Where(w => !w.StartsWith("_")).ToList().ForEach(f =>
            {
                set.Add($"{f} = @{f}");
            });
            return $"UPDATE {GetTableName<T>()} SET {string.Join(",", set)} {where} ";
        }
        else
        {
            return string.Empty;
        }
    }
    public static string CreateDelete<T>(string where) where T : class
    {
        if (!string.IsNullOrEmpty(where))
        {
            return $"DELETE FROM {GetTableName<T>()} {where} ";
        }
        else
        {
            return string.Empty;
        }
    }
}
