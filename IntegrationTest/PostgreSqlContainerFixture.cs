using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace IntegrationTest;

/// <summary>
/// Assembly 級 PostgreSQL container fixture（template database 模式）：
/// <list type="bullet">
/// <item><see cref="AssemblyInitialize"/> 啟動 <c>postgres:17-alpine</c>，建立 template database，
/// 對其跑一次 EF Core MigrateAsync 與 DropConstraintScript，並建立共用 <see cref="Respawner"/>。</item>
/// <item><see cref="EnsureDatabaseAsync"/> 為每個 test class 以 <c>CREATE DATABASE ... TEMPLATE</c>
/// 複製 template database（檔案層級複製，免去重跑遷移）。</item>
/// <item><see cref="ResetDatabaseAsync"/> 清空業務表資料但保留 <c>__ef_migrations_history</c>。</item>
/// <item><see cref="CreateDbContext"/> 以 <c>Database=&lt;database&gt;</c> 取得 database 隔離的 DbContext。</item>
/// </list>
/// </summary>
[TestClass]
public static class PostgreSqlContainerFixture
{
    private const string MigrationsHistoryTable = "__ef_migrations_history";
    private const string TemplateDatabase = "app_template";
    private const string DefaultSchema = "public";

    private static PostgreSqlContainer? _container;

    /// <summary>所有 clone database 結構相同，Respawner 的 introspection 只需做一次即可共用。</summary>
    private static Respawner? _respawner;

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

    public static string GetDatabaseConnectionString(string database) =>
        new NpgsqlConnectionStringBuilder(ConnectionString) { Database = database }.ConnectionString;

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

        await ExecuteAdminCommandAsync($"CREATE DATABASE \"{TemplateDatabase}\";");

        // Pooling=false:避免殘留的 pooled connection 佔住 template database,
        // 導致後續 CREATE DATABASE ... TEMPLATE 失敗。
        var templateConnectionString = new NpgsqlConnectionStringBuilder(
            GetDatabaseConnectionString(TemplateDatabase))
        {
            Pooling = false,
        }.ConnectionString;

        await using (var dbContext = CreateDbContextCore(templateConnectionString))
        {
            await dbContext.Database.MigrateAsync();
            await dbContext.Database.ExecuteSqlRawAsync(DbMaintenanceScript.DropConstraintScript);
        }

        await using (var respawnConn = new NpgsqlConnection(templateConnectionString))
        {
            await respawnConn.OpenAsync();
            _respawner = await Respawner.CreateAsync(
                respawnConn,
                new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres,
                    SchemasToInclude = [DefaultSchema],
                    TablesToIgnore = [new(DefaultSchema, MigrationsHistoryTable)],
                }
            );
        }

        await ExecuteAdminCommandAsync(
            $"ALTER DATABASE \"{TemplateDatabase}\" WITH ALLOW_CONNECTIONS false IS_TEMPLATE true;");
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

    public static async Task EnsureDatabaseAsync(string database)
    {
        if (database.Contains('"'))
            throw new ArgumentException($"Database name must not contain double quotes: '{database}'", nameof(database));

        // CREATE/DROP DATABASE 不能在 transaction 內執行,必須逐一送出。
        await ExecuteAdminCommandAsync($"DROP DATABASE IF EXISTS \"{database}\" WITH (FORCE);");
        await ExecuteAdminCommandAsync($"CREATE DATABASE \"{database}\" TEMPLATE \"{TemplateDatabase}\";");
    }

    public static async Task ResetDatabaseAsync(string database)
    {
        if (_respawner is null)
            throw new InvalidOperationException("ResetDatabaseAsync called before AssemblyInitialize completed.");

        await using var conn = new NpgsqlConnection(GetDatabaseConnectionString(database));
        await conn.OpenAsync();
        await _respawner.ResetAsync(conn);
    }

    public static ApplicationDbContext CreateDbContext(string database) =>
        CreateDbContextCore(GetDatabaseConnectionString(database));

    private static ApplicationDbContext CreateDbContextCore(string connectionString)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(
                connectionString,
                opts =>
                {
                    opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opts.MinBatchSize(10);
                    opts.MaxBatchSize(1000);
                    opts.MigrationsHistoryTable(MigrationsHistoryTable);
                })
            .UseSnakeCaseNamingConvention()
            .UseLoggerFactory(_loggerFactory)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

        return new ApplicationDbContext(dbContextOptionsBuilder.Options);
    }

    private static async Task ExecuteAdminCommandAsync(string sql)
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }
}
