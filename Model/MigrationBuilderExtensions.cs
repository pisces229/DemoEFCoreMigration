using Microsoft.EntityFrameworkCore.Migrations;
using Model.Scripts;

namespace Model;

public static class MigrationBuilderExtensions
{
    public static void CreateScripts(this MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(FuncScalar.Create);
        migrationBuilder.Sql(FuncScalarWithParam.Create);
        migrationBuilder.Sql(FuncTable.Create);
        migrationBuilder.Sql(FuncTableWithParam.Create);
        migrationBuilder.Sql(Proc.Create);
        migrationBuilder.Sql(View.Create);
    }
}
