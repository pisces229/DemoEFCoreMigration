using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Model;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DbMigration;

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage")]
public class UsageNpgsqlMigrationsSqlGenerator(
    MigrationsSqlGeneratorDependencies dependencies,
    INpgsqlSingletonOptions npgsqlOptions,
    IRelationalCommandDiagnosticsLogger logger)
    : NpgsqlMigrationsSqlGenerator(dependencies, npgsqlOptions)
{
    protected override void Generate(
        MigrationOperation operation,
        IModel? model,
        MigrationCommandListBuilder builder)
    {
        base.Generate(operation, model, builder);
    }

    public override IReadOnlyList<MigrationCommand> Generate(
        IReadOnlyList<MigrationOperation> operations,
        IModel? model,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        var commands = base.Generate(operations, model, options).ToList();

        var commandBuilder = Dependencies.CommandBuilderFactory.Create();
        commandBuilder.Append(DbMaintenanceScript.CreatePlpgsqlCheckScript);
        commandBuilder.Append(DbMaintenanceScript.ExcutePlpgsqlCheckScript);
        var relationalCommand = commandBuilder.Build();

        commands.Add(new MigrationCommand(
            relationalCommand,
            context: null,
            logger: logger,
            transactionSuppressed: false
        ));

        return commands;
    }

    protected override void Generate(
        AddForeignKeyOperation operation,
        IModel? model,
        MigrationCommandListBuilder builder,
        bool terminate = true)
    {
        base.Generate(operation, model, builder, terminate);

        if (!terminate) return;
        var comment = LookupForeignKeyComment(model, operation.Schema, operation.Table, operation.Name);
        AppendCommentOnConstraint(builder, comment, operation.Schema, operation.Table, operation.Name);
    }

    protected override void Generate(
        AddPrimaryKeyOperation operation,
        IModel? model,
        MigrationCommandListBuilder builder,
        bool terminate = true)
    {
        base.Generate(operation, model, builder, terminate);

        if (!terminate) return;
        var comment = LookupKeyComment(model, operation.Schema, operation.Table, operation.Name, isPrimary: true);
        AppendCommentOnConstraint(builder, comment, operation.Schema, operation.Table, operation.Name);
    }

    protected override void Generate(
        AddUniqueConstraintOperation operation,
        IModel? model,
        MigrationCommandListBuilder builder)
    {
        base.Generate(operation, model, builder);

        var comment = LookupKeyComment(model, operation.Schema, operation.Table, operation.Name, isPrimary: false);
        AppendCommentOnConstraint(builder, comment, operation.Schema, operation.Table, operation.Name);
    }

    protected override void Generate(
        CreateIndexOperation operation,
        IModel? model,
        MigrationCommandListBuilder builder,
        bool terminate = true)
    {
        base.Generate(operation, model, builder, terminate);

        if (!terminate) return;
        var comment = LookupIndexComment(model, operation.Schema, operation.Table, operation.Name);
        AppendCommentOnIndex(builder, comment, operation.Schema, operation.Name);
    }

    protected override void Generate(
        CreateTableOperation operation,
        IModel? model,
        MigrationCommandListBuilder builder,
        bool terminate = true)
    {
        base.Generate(operation, model, builder, terminate);

        if (!terminate) return;

        var entityType = FindEntityType(model, operation.Schema, operation.Name);

        if (operation.PrimaryKey is not null)
        {
            AppendCommentOnConstraint(
                builder, CommentForKey(entityType, operation.PrimaryKey.Name, isPrimary: true),
                operation.PrimaryKey.Schema ?? operation.Schema,
                operation.PrimaryKey.Table ?? operation.Name,
                operation.PrimaryKey.Name);
        }

        foreach (var uk in operation.UniqueConstraints)
        {
            AppendCommentOnConstraint(
                builder, CommentForKey(entityType, uk.Name, isPrimary: false),
                uk.Schema ?? operation.Schema,
                uk.Table ?? operation.Name,
                uk.Name);
        }

        foreach (var fk in operation.ForeignKeys)
        {
            AppendCommentOnConstraint(
                builder, CommentForForeignKey(entityType, fk.Name),
                fk.Schema ?? operation.Schema,
                fk.Table ?? operation.Name,
                fk.Name);
        }
    }

    private static IEntityType? FindEntityType(IModel? model, string? schema, string tableName)
    {
        if (model is null) return null;
        return model.GetEntityTypes()
            .FirstOrDefault(e =>
                e.GetTableName() == tableName
                && (schema is null || e.GetSchema() == schema || (e.GetSchema() is null && schema == e.GetDefaultSchema())));
    }

    private static string? CommentForKey(IEntityType? entityType, string keyName, bool isPrimary)
    {
        if (entityType is null) return null;

        var key = isPrimary
            ? entityType.FindPrimaryKey()
            : entityType.GetKeys().FirstOrDefault(k => !k.IsPrimaryKey() && k.GetName() == keyName);

        if (key is null) return null;
        if (key.GetName() != keyName) return null;

        return BuildKeyComment(key);
    }

    private static string? CommentForForeignKey(IEntityType? entityType, string fkName) =>
        entityType?.GetForeignKeys().FirstOrDefault(f => f.GetConstraintName() == fkName) is { } fk
            ? BuildForeignKeyComment(fk)
            : null;

    private static string? CommentForIndex(IEntityType? entityType, string indexName) =>
        entityType?.GetIndexes().FirstOrDefault(i => i.GetDatabaseName() == indexName) is { } index
            ? BuildIndexComment(index)
            : null;

    private static string? LookupKeyComment(IModel? model, string? schema, string table, string keyName, bool isPrimary) =>
        CommentForKey(FindEntityType(model, schema, table), keyName, isPrimary);

    private static string? LookupForeignKeyComment(IModel? model, string? schema, string table, string fkName) =>
        CommentForForeignKey(FindEntityType(model, schema, table), fkName);

    private static string? LookupIndexComment(IModel? model, string? schema, string table, string indexName) =>
        CommentForIndex(FindEntityType(model, schema, table), indexName);

    /// <summary>
    /// 取 EntityType 的可讀短名並轉 snake_case。對 shared-type / property-bag entity
    /// (例如 EF Core 自動產生的 M2M join entity,其 ClrType 是 Dictionary&lt;string, object&gt;)
    /// 改取 entityType.Name 的最後一段使用者命名,避免出現 Dictionary`2 等沒意義的字串。
    /// </summary>
    private static string GetEntityShortName(IReadOnlyEntityType entityType)
    {
        string raw;
        if (entityType.HasSharedClrType || entityType.ClrType == typeof(Dictionary<string, object>))
        {
            var name = entityType.Name;
            var lastDot = name.LastIndexOf('.');
            var lastPlus = name.LastIndexOf('+');
            var cut = Math.Max(lastDot, lastPlus);
            raw = cut >= 0 ? name[(cut + 1)..] : name;
        }
        else
        {
            raw = entityType.ClrType.Name;
        }
        return DbContextUtil.NamingConvention(raw);
    }

    private static string JoinSnakeCaseCols(IEnumerable<IReadOnlyProperty> properties) =>
        string.Join(",", properties.Select(p => DbContextUtil.NamingConvention(p.Name)));

    /// <summary>
    /// 以 EntityType 短名 + Property 欄位名組 Comment(snake_case,空白分隔)。
    /// 格式:pk {entity} {cols} / ak {entity} {cols}(欄位之間以 "," 串接)
    /// </summary>
    private static string BuildKeyComment(IReadOnlyKey key)
    {
        var entityName = GetEntityShortName(key.DeclaringEntityType);
        var cols = JoinSnakeCaseCols(key.Properties);
        var prefix = key.IsPrimaryKey() ? "pk" : "ak";
        return $"{prefix} {entityName} {cols}";
    }

    /// <summary>
    /// 格式:fk {entity} {cols} {principal_entity} {principal_cols}
    /// 同時呈現本表(declaring)欄位與所引用主表(principal)欄位。
    /// </summary>
    private static string BuildForeignKeyComment(IReadOnlyForeignKey fk)
    {
        var entityName = GetEntityShortName(fk.DeclaringEntityType);
        var principalName = GetEntityShortName(fk.PrincipalEntityType);
        var cols = JoinSnakeCaseCols(fk.Properties);
        var principalCols = JoinSnakeCaseCols(fk.PrincipalKey.Properties);
        return $"fk {entityName} {cols} {principalName} {principalCols}";
    }

    /// <summary>
    /// 格式:ix {entity} {cols}(Unique index 同樣使用 ix 前綴,與 EF Core 預設一致)
    /// </summary>
    private static string BuildIndexComment(IReadOnlyIndex index)
    {
        var entityName = GetEntityShortName(index.DeclaringEntityType);
        var cols = JoinSnakeCaseCols(index.Properties);
        return $"ix {entityName} {cols}";
    }

    private void AppendCommentOnConstraint(
        MigrationCommandListBuilder builder,
        string? comment,
        string? schema,
        string table,
        string constraintName)
    {
        if (string.IsNullOrWhiteSpace(comment)) return;

        var sqlHelper = Dependencies.SqlGenerationHelper;
        builder
            .Append("COMMENT ON CONSTRAINT ")
            .Append(sqlHelper.DelimitIdentifier(constraintName))
            .Append(" ON ")
            .Append(sqlHelper.DelimitIdentifier(table, schema))
            .Append(" IS ")
            .Append(GenerateSqlLiteral(comment))
            .AppendLine(sqlHelper.StatementTerminator);

        builder.EndCommand();
    }

    private void AppendCommentOnIndex(
        MigrationCommandListBuilder builder,
        string? comment,
        string? schema,
        string indexName)
    {
        if (string.IsNullOrWhiteSpace(comment)) return;

        var sqlHelper = Dependencies.SqlGenerationHelper;
        builder
            .Append("COMMENT ON INDEX ")
            .Append(sqlHelper.DelimitIdentifier(indexName, schema))
            .Append(" IS ")
            .Append(GenerateSqlLiteral(comment))
            .AppendLine(sqlHelper.StatementTerminator);

        builder.EndCommand();
    }

    private string GenerateSqlLiteral(string value)
    {
        var mapping = Dependencies.TypeMappingSource.FindMapping(typeof(string))
            ?? throw new InvalidOperationException("No relational type mapping found for System.String.");
        return mapping.GenerateSqlLiteral(value);
    }
}
