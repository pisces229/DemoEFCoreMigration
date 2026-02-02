using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
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

        switch (configurationRoot.GetValue<string>("DbContext")!)
        {
            case "SqlServer":
                dbContextOptionsBuilder.UseSqlServer(configurationRoot.GetConnectionString("SqlServer"));
                break;
            case "PostgreSQL":
                dbContextOptionsBuilder.UseNpgsql(configurationRoot.GetConnectionString("PostgreSQL"),
                    t => t.MigrationsHistoryTable("__ef_migrations_history"))
                    .UseSnakeCaseNamingConvention()
                    .ReplaceService<IMigrationsSqlGenerator, UsageNpgsqlMigrationsSqlGenerator>();
                break;
            case "Sqlite":
                dbContextOptionsBuilder.UseSqlite(configurationRoot.GetConnectionString("Sqlite"));
                break;
        }
        dbContextOptionsBuilder.EnableDetailedErrors();
        dbContextOptionsBuilder.EnableSensitiveDataLogging();
        dbContextOptionsBuilder.LogTo(Console.WriteLine, LogLevel.Warning);
        return new ApplicationDbContext(dbContextOptionsBuilder.Options);
    }
}
