using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;

namespace DbMigration;

public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var dbContextValue = configurationRoot.GetValue<string>("DbContext");
        if (string.IsNullOrWhiteSpace(dbContextValue))
        {
            throw new InvalidOperationException("DbContext configuration is missing or empty. Set 'DbContext' in appsettings.json to 'SqlServer', 'PostgreSQL', or 'Sqlite'.");
        }

        switch (dbContextValue)
        {
            case "SqlServer":
                dbContextOptionsBuilder.UseSqlServer(configurationRoot.GetConnectionString("SqlServer"));
                break;
            case "PostgreSQL":
                dbContextOptionsBuilder.UseNpgsql(configurationRoot.GetConnectionString("PostgreSQL"),
                    t => t.MigrationsHistoryTable("__ef_migrations_history"))
                    .UseSnakeCaseNamingConvention();
                //.ReplaceService<IMigrationsSqlGenerator, UsageNpgsqlMigrationsSqlGenerator>();
                break;
            case "Sqlite":
                dbContextOptionsBuilder.UseSqlite(configurationRoot.GetConnectionString("Sqlite"));
                break;
            default:
                throw new InvalidOperationException($"Unknown DbContext value: '{dbContextValue}'. Valid values are: 'SqlServer', 'PostgreSQL', 'Sqlite'.");
        }
        dbContextOptionsBuilder.EnableDetailedErrors();

        // Only enable sensitive data logging in development
        var isDevelopment = string.Equals(
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            "Development",
            StringComparison.OrdinalIgnoreCase);
        if (isDevelopment)
        {
            dbContextOptionsBuilder.EnableSensitiveDataLogging();
        }

        dbContextOptionsBuilder.LogTo(Console.WriteLine, LogLevel.Warning);
        return new ApplicationDbContext(dbContextOptionsBuilder.Options);
    }
}
