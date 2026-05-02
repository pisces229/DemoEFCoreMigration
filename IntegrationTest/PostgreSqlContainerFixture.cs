using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql;
using Respawn;
using System.Collections.Concurrent;
using Testcontainers.PostgreSql;

namespace IntegrationTest;

/// <summary>
/// Assembly 級 PostgreSQL container fixture：
/// <list type="bullet">
/// <item><see cref="AssemblyInitialize"/> 啟動 <c>postgres:17-alpine</c>。</item>
/// <item><see cref="EnsureSchemaAsync"/> 為每個 test class 建立獨立 schema 並跑 EF Core MigrateAsync，
/// 同時建立該 schema 專屬的 <see cref="Respawner"/>。</item>
/// <item><see cref="ResetSchemaAsync"/> 清空業務表資料但保留 <c>__ef_migrations_history</c>。</item>
/// <item><see cref="CreateDbContext"/> 以 <c>SearchPath=&lt;schema&gt;</c> 取得 schema 隔離的 DbContext。</item>
/// </list>
/// </summary>
[TestClass]
public static class PostgreSqlContainerFixture
{
    private const string MigrationsHistoryTable = "__ef_migrations_history";

    private static PostgreSqlContainer? _container;
    private static readonly ConcurrentDictionary<string, Respawner> _respawners = new();

    private static readonly ILoggerFactory _loggerFactory =
        Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(LogLevel.Information)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddProvider(new SyncConsoleLoggerProvider());
        });

    public static ILoggerFactory LoggerFactory => _loggerFactory;

    public static string ConnectionString => _container?.GetConnectionString()
        ?? throw new InvalidOperationException("PostgreSQL container has not been started.");

    public static string GetSchemaConnectionString(string schema) =>
        new NpgsqlConnectionStringBuilder(ConnectionString) { SearchPath = schema }.ConnectionString;

    [AssemblyInitialize]
    public static async Task AssemblyInitialize(TestContext _)
    {
        _container = new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("postgres")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithEnvironment("TZ", "UTC")
            .Build();

        await _container.StartAsync();
    }

    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        if (_container is not null)
        {
            await _container.DisposeAsync();
            _container = null;
        }
    }

    public static async Task EnsureSchemaAsync(string schema)
    {
        if (schema.Contains('"'))
            throw new ArgumentException($"Schema name must not contain double quotes: '{schema}'", nameof(schema));

        await using (var conn = new NpgsqlConnection(ConnectionString))
        {
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                $"DROP SCHEMA IF EXISTS \"{schema}\" CASCADE; CREATE SCHEMA \"{schema}\";",
                conn
            );
            await cmd.ExecuteNonQueryAsync();
        }

        await using (var dbContext = CreateDbContext(schema))
        {
            await dbContext.Database.MigrateAsync();
            await dbContext.Database.ExecuteSqlRawAsync(DbMaintenanceScript.DropConstraintScript);
        }

        await using (var respawnConn = new NpgsqlConnection(GetSchemaConnectionString(schema)))
        {
            await respawnConn.OpenAsync();
            _respawners[schema] = await Respawner.CreateAsync(
                respawnConn,
                new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres,
                    SchemasToInclude = [schema],
                    TablesToIgnore = [new(schema, MigrationsHistoryTable)],
                }
            );
        }
    }

    public static async Task ResetSchemaAsync(string schema)
    {
        if (!_respawners.TryGetValue(schema, out var respawner))
            throw new InvalidOperationException($"ResetSchemaAsync called before EnsureSchemaAsync for schema '{schema}'.");

        await using var conn = new NpgsqlConnection(GetSchemaConnectionString(schema));
        await conn.OpenAsync();
        await respawner.ResetAsync(conn);
    }

    public static ApplicationDbContext CreateDbContext(string schema)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(
                GetSchemaConnectionString(schema),
                opts =>
                {
                    opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opts.MinBatchSize(10);
                    opts.MaxBatchSize(1000);
                    opts.MigrationsHistoryTable(MigrationsHistoryTable, schema);
                })
            .UseSnakeCaseNamingConvention()
            .UseLoggerFactory(_loggerFactory)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

        return new ApplicationDbContext(dbContextOptionsBuilder.Options);
    }
}
