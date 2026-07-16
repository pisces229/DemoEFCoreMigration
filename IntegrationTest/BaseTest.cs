using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace IntegrationTest;

/// <summary>
/// 測試基底:每個衍生 class 自動從 template database 複製一個獨立 PostgreSQL database,
/// <see cref="TestInitialize"/> 提供乾淨的 <see cref="ApplicationDbContext"/>。
/// </summary>
[TestClass]
public abstract class BaseTest
{
    private static readonly ConcurrentDictionary<string, string> _databaseByClass = new();

    private string _database = null!;

    protected ApplicationDbContext _dbContext { get; private set; } = null!;
    protected ILogger _logger { get; private set; } = null!;

    protected ApplicationDbContext CreateDbContext() =>
        PostgreSqlContainerFixture.CreateDbContext(_database);

    [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
    public static async Task BaseClassInitialize(TestContext testContext)
    {
        var className = testContext.FullyQualifiedTestClassName!;
        var database = ToDatabaseName(className);
        _databaseByClass[className] = database;
        await PostgreSqlContainerFixture.EnsureDatabaseAsync(database);
    }

    [TestInitialize]
    public async Task TestInitialize()
    {
        _database = _databaseByClass[GetType().FullName!];
        await PostgreSqlContainerFixture.ResetDatabaseAsync(_database);
        _dbContext = PostgreSqlContainerFixture.CreateDbContext(_database);
        _logger = PostgreSqlContainerFixture.LoggerFactory.CreateLogger(GetType());
    }

    [TestCleanup]
    public virtual async Task TestCleanup()
    {
        await _dbContext.DisposeAsync();
    }

    private static string ToDatabaseName(string fullClassName)
    {
        var name = fullClassName.Replace('.', '_').ToLowerInvariant();
        if (name.Length <= DbContextUtil.MaxNameLength)
            return name;
        return name[..(DbContextUtil.MaxNameLength - DbContextUtil.HashNameLength)]
            + DbContextUtil.ToHashName(name);
    }
}
