using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DbMigration;

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage")]
public class UsageNpgsqlMigrationsSqlGenerator(
    MigrationsSqlGeneratorDependencies dependencies,
    INpgsqlSingletonOptions npgsqlOptions,
    IRelationalCommandDiagnosticsLogger logger,
    IMigrationsAssembly migrationsAssembly)
    : NpgsqlMigrationsSqlGenerator(dependencies, npgsqlOptions)
{
    protected override void Generate(
        MigrationOperation operation,
        IModel? model,
        MigrationCommandListBuilder builder)
    {
        //Console.WriteLine("UsageNpgsqlMigrationsSqlGenerator void Generate.");

        base.Generate(operation, model, builder);
    }

    public override IReadOnlyList<MigrationCommand> Generate(
        IReadOnlyList<MigrationOperation> operations,
        IModel? model,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        //Console.WriteLine("UsageNpgsqlMigrationsSqlGenerator IReadOnlyList<MigrationCommand> Generate.");

        var commands = base.Generate(operations, model, options).ToList();

        foreach (var migrationId in migrationsAssembly.Migrations.Keys)
        {
            //Console.WriteLine("MigrationId:" + migrationId);
        }

        if (migrationsAssembly.Migrations.Keys.Count() == 0)
        {
            var commandBuilder = Dependencies.CommandBuilderFactory.Create();
            commandBuilder.Append("SELECT 1");
            var relationalCommand = commandBuilder.Build();

            commands.Add(new MigrationCommand(
                relationalCommand,
                context: null,
                logger: logger,
                transactionSuppressed: false
            ));
        }
        return commands;
    }
}
