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
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        dbContextOptionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        dbContextOptionsBuilder.EnableSensitiveDataLogging();
        // (Lazy loading)
        //dbContextOptionsBuilder.UseLazyLoadingProxies();

        switch (configurationRoot.GetValue<string>("DbContext")!)
        {
            case "SqlServer":
                dbContextOptionsBuilder.UseSqlServer(configurationRoot.GetConnectionString("SqlServer"),
                    opts =>
                    {
                        opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        opts.MinBatchSize(10);
                        opts.MaxBatchSize(1000);
                    });
                break;
            case "PostgreSQL":
                dbContextOptionsBuilder.UseNpgsql(configurationRoot.GetConnectionString("PostgreSQL"),
                    opts =>
                    {
                        opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        opts.MinBatchSize(10);
                        opts.MaxBatchSize(1000);
                    })
                    .UseSnakeCaseNamingConvention();
                break;
            case "Sqlite":
                dbContextOptionsBuilder.UseSqlite(configurationRoot.GetConnectionString("Sqlite"),
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
    public async Task TestCleanup()
    {
        Console.WriteLine("TestCleanup");
        await _dbContext.DisposeAsync();
    }
}
