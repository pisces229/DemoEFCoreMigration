using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace IntegrationTest;

/// <summary>
/// 測試基底:每個衍生 class 自動配置一個獨立 PostgreSQL schema,
/// <see cref="TestInitialize"/> 提供乾淨的 <see cref="ApplicationDbContext"/>。
/// </summary>
[TestClass]
public abstract class BaseTest
{
    private static readonly ConcurrentDictionary<string, string> _schemaByClass = new();

    private string _schema = null!;

    protected ApplicationDbContext _dbContext { get; private set; } = null!;
    protected ILogger _logger { get; private set; } = null!;

    protected ApplicationDbContext CreateDbContext() =>
        PostgreSqlContainerFixture.CreateDbContext(_schema);

    [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
    public static async Task BaseClassInitialize(TestContext testContext)
    {
        var className = testContext.FullyQualifiedTestClassName!;
        var schema = ToSchemaName(className);
        _schemaByClass[className] = schema;
        await PostgreSqlContainerFixture.EnsureSchemaAsync(schema);
    }

    [TestInitialize]
    public async Task TestInitialize()
    {
        _schema = _schemaByClass[GetType().FullName!];
        await PostgreSqlContainerFixture.ResetSchemaAsync(_schema);
        _dbContext = PostgreSqlContainerFixture.CreateDbContext(_schema);
        _logger = PostgreSqlContainerFixture.LoggerFactory.CreateLogger(GetType());
    }

    [TestCleanup]
    public virtual async Task TestCleanup()
    {
        await _dbContext.DisposeAsync();
    }

    private static string ToSchemaName(string fullClassName)
    {
        var name = fullClassName.Replace('.', '_').ToLowerInvariant();
        if (name.Length <= DbContextUtil.MaxNameLength) return name;

        var suffix = "_" + DbContextUtil.ToHashName(fullClassName);
        return name[..(DbContextUtil.MaxNameLength - suffix.Length)] + suffix;
    }
}
