using Dapper;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.DapperUtil;

public static class SqlStatementUtility
{
    public static string GetSqlString(string sql)
    {
        return new Regex("[ ]{2,}", RegexOptions.None).Replace(sql.Replace(Environment.NewLine, " "), " ");
    }
    public static string GetDynamicParameters(DynamicParameters? dynamicParameters)
    {
        var result = new StringBuilder();
        if (dynamicParameters != null)
        {
            var parameterNames = dynamicParameters.ParameterNames.ToList();
            if (parameterNames.Any())
            {
                result.Append("-- parameters:");
                parameterNames.ForEach(name =>
                {
                    var dynamicParameter = dynamicParameters.Get<object>(name);
                    if (dynamicParameter != null)
                    {
                        var dynamicParameterType = dynamicParameter.GetType();
                        if (dynamicParameter is IList list)
                        {
                            var stringBuilder = new StringBuilder();
                            foreach (var value in list)
                            {
                                if (stringBuilder.Length > 0)
                                {
                                    stringBuilder.Append(',');
                                }
                                stringBuilder.Append(GetParameterValue(value));
                            }
                            result.Append($"[{name}]:[{stringBuilder.ToString()}],");
                        }
                        else
                        {
                            result.Append($"[{name}]:[{GetParameterValue(dynamicParameter)}],");
                        }
                    }
                    else
                    {
                        result.Append($"[{name}]:[NULL],");
                    }
                });
            }
        }
        return result.ToString();
    }
    private static string GetParameterValue(object value)
    {
        if (value == null)
            return "NULL";
        else if (value is DateTime dateTime)
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        else
            return value.ToString()!;
    }
}
