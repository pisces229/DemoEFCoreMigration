using Dapper;

namespace Common.DapperUtil;

public static class SqlStatementService
{
    public static string CreateSelect<T>(string where) where T : class
    {
        return $"SELECT * FROM {typeof(T).Name} {where}";
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
        return $"INSERT INTO {typeof(T).Name} ({string.Join(",", insert)}) VALUES ({string.Join(",", values)})";
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
            return $"UPDATE {typeof(T).Name} SET {string.Join(",", set)} {where} ";
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
            return $"DELETE FROM {typeof(T).Name} {where} ";
        }
        else
        {
            return string.Empty;
        }
    }
}
