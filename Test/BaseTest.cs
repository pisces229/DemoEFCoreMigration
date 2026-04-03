using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;

namespace Test;

[TestClass]
public class BaseTest
{
    protected ApplicationDbContext _dbContext { get; set; } = null!;

    [TestInitialize]
    public Task TestInitialize()
    {
        Console.WriteLine("TestInitialize");
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .AddJsonFile($"appSettings.{environment}.json", optional: true) // Load environment-specific config
            .AddEnvironmentVariables() // Load environment variables (overrides file config)
            .Build();

        // Replace password placeholders with environment variables
        var connectionStrings = configurationRoot.GetSection("ConnectionStrings").Get<Dictionary<string, string>>()
            ?? new Dictionary<string, string>();

        foreach (var key in connectionStrings.Keys.ToList())
        {
            var connStr = connectionStrings[key];
            connStr = connStr?.Replace("{SQLSERVER_PASSWORD}", Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD") ?? "")
                              .Replace("{POSTGRESQL_PASSWORD}", Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD") ?? "")
                ?? "";
            connectionStrings[key] = connStr;
        }

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        dbContextOptionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        // Only enable sensitive data logging in development
        var isDevelopment = string.Equals(
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            "Development",
            StringComparison.OrdinalIgnoreCase);
        if (isDevelopment)
        {
            dbContextOptionsBuilder.EnableSensitiveDataLogging();
        }

        // (Lazy loading)
        //dbContextOptionsBuilder.UseLazyLoadingProxies();

        switch (configurationRoot.GetValue<string>("DbContext")!)
        {
            case "SqlServer":
                dbContextOptionsBuilder.UseSqlServer(connectionStrings["SqlServer"],
                    opts =>
                    {
                        opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        opts.MinBatchSize(10);
                        opts.MaxBatchSize(1000);
                    });
                break;
            case "PostgreSQL":
                dbContextOptionsBuilder.UseNpgsql(connectionStrings["PostgreSQL"],
                    opts =>
                    {
                        opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        opts.MinBatchSize(10);
                        opts.MaxBatchSize(1000);
                    })
                    .UseSnakeCaseNamingConvention();
                break;
            case "Sqlite":
                dbContextOptionsBuilder.UseSqlite(connectionStrings["Sqlite"],
                    opts =>
                    {
                        opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        opts.MinBatchSize(10);
                        opts.MaxBatchSize(1000);
                    });
                break;
        }

        _dbContext = new ApplicationDbContext(dbContextOptionsBuilder.Options);

        // This switch reenables legacy behavior by removing strict DateTime.Kind validation, allowing UTC timestamps to be written to PostgreSQL timestamp fields.
        // AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return Task.CompletedTask;
    }

    [TestCleanup]
    public virtual async Task TestCleanup()
    {
        Console.WriteLine("TestCleanup");
        await _dbContext.DisposeAsync();
    }
}
