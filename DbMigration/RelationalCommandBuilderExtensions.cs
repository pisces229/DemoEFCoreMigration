using Microsoft.EntityFrameworkCore.Storage;

namespace DbMigration;

public static class RelationalCommandBuilderExtensions
{
    public static void AppendScripts(this IRelationalCommandBuilder commandBuilder)
    {
        // do something...
    }
}
