using Model.Entities;
using System.Text.Json;

namespace Model;

/// <summary>
/// SqlServer, PostgreSQL, Sqlite
/// </summary>
internal class DbContextUtil
{
    public const string Schema = "public";

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string ToSnakeCase(string name) =>
        string.Concat(name.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())).ToLower();

    public static string CreateForeignKey(string table, string name) =>
        string.Format("fk__{0}__{1}", ToSnakeCase(table), ToSnakeCase(name));

    public static string DropConstraintScript(string table, string name) =>
        string.Format("ALTER TABLE IF EXISTS {0} DROP CONSTRAINT IF EXISTS {1};", ToSnakeCase(table), name);
}
