
namespace DbMigration;

internal static class Util
{
    /// <summary>
    /// Extracts the migration name from a migration identifier.
    /// Input format: "20260329153041_Init" -> Output: "Init"
    /// </summary>
    public static string GetMigrationName(string migration)
    {
        var underscoreIndex = migration.IndexOf('_');
        if (underscoreIndex < 0)
        {
            throw new InvalidOperationException($"Migration name '{migration}' does not contain an underscore separator.");
        }
        return migration[(underscoreIndex + 1)..];
    }
}
