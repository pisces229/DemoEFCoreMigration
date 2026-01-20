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

    public static string NamingConvention(string name) => ToSnakeCase(name);

    public static string ToSnakeCase(string name) =>
        string.Concat(name.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())).ToLower();

    public static string CreateIndexKey(string table, string name) =>
        $"ix__{ToSnakeCase(table)}__{ToSnakeCase(name)}";

    public static string CreateForeignKey(string table, string name) =>
        $"fk__{ToSnakeCase(table)}__{ToSnakeCase(name)}";

    public static string DropConstraintScript(string table, string name) =>
        $"ALTER TABLE IF EXISTS {ToSnakeCase(table)} DROP CONSTRAINT IF EXISTS {ToSnakeCase(name)};";

    public static string DropConstraintScript(string table, IEnumerable<string> names) =>
        string.Join(Environment.NewLine, names.Select(name => $"ALTER TABLE IF EXISTS {ToSnakeCase(table)} DROP CONSTRAINT IF EXISTS {ToSnakeCase(name)};"));
}
