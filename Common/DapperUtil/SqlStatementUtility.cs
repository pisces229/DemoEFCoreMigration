using Dapper;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.DapperUtil;

public static partial class SqlStatementUtility
{
    [GeneratedRegex("[ ]{2,}")]
    private static partial Regex MultiSpaceRegex();

    public static string GetSqlString(string sql)
    {
        return MultiSpaceRegex().Replace(sql.Replace(Environment.NewLine, " "), " ");
    }
    public static string GetDynamicParameters(DynamicParameters? dynamicParameters)
    {
        if (dynamicParameters is null) return string.Empty;

        var parameterNames = dynamicParameters.ParameterNames as ICollection<string> ?? dynamicParameters.ParameterNames.ToList();
        if (parameterNames.Count == 0) return string.Empty;

        var result = new StringBuilder("-- parameters:");
        foreach (var name in parameterNames)
        {
            var dynamicParameter = dynamicParameters.Get<object>(name);
            if (dynamicParameter is null)
            {
                result.Append($"[{name}]:[NULL],");
                continue;
            }
            if (dynamicParameter is IList list)
            {
                var stringBuilder = new StringBuilder();
                foreach (var value in list)
                {
                    if (stringBuilder.Length > 0) stringBuilder.Append(',');
                    stringBuilder.Append(GetParameterValue(value));
                }
                result.Append($"[{name}]:[{stringBuilder}],");
            }
            else
            {
                result.Append($"[{name}]:[{GetParameterValue(dynamicParameter)}],");
            }
        }
        return result.ToString();
    }
    private static string GetParameterValue(object value)
    {
        if (value == null) return "NULL";
        if (value is DateTime dateTime) return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        return value.ToString()!;
    }
}
