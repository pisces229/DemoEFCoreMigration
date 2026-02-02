
namespace DbMigration;

internal static class Util
{
    public static string GetMigrationName(string migration) => migration[(migration.IndexOf('_') + 1)..];
}
