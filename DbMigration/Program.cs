using DbMigration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Model;

try
{
    Console.WriteLine("DbMigration Start...");

    var dbContext = new DbContextFactory().CreateDbContext(args);

    //var migrations = context.Database.GetMigrations();
    //foreach (var migration in migrations)
    //{
    //    Console.WriteLine($"DbMigration: {migration}...");
    //}

    var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
    if (!appliedMigrations.Any())
    {
        Console.WriteLine("Database has no migrations applied...");
        await dbContext.Database.MigrateAsync();
        await new SeedData(dbContext).RunAsync();
    }
    else
    {
        const string commandLineArg = "TargetMigration=";
        var targetMigration = Environment.GetCommandLineArgs()
            .FirstOrDefault(w => w.StartsWith(commandLineArg))?.Replace(commandLineArg, "");
        if (!string.IsNullOrEmpty(targetMigration))
        {
            Console.WriteLine($"Database updating to {targetMigration}...");
            await dbContext.Database.GetService<IMigrator>().MigrateAsync(Util.GetMigrationName(targetMigration));
        }
        else
        {
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                Console.WriteLine("Database has pending migrations...");
                foreach (var pendingMigration in pendingMigrations)
                {
                    Console.WriteLine($"PendingMigration: {pendingMigration}");
                }
                await dbContext.Database.GetService<IMigrator>().MigrateAsync();
                //foreach (var pendingMigration in pendingMigrations)
                //{
                //    Console.WriteLine($"Database updated to {pendingMigration}...");
                //    await  context.Database.GetService<IMigrator>().MigrateAsync(Util.GetMigrationName(pendingMigration));
                //}
            }
            else
            {
                Console.WriteLine("Database is up to date...");
            }
        }
    }
    // DropConstraintScript
    {
        Console.WriteLine("DropConstraintScript:" + DbMaintenanceScript.DropConstraintScript);
        await dbContext.Database.ExecuteSqlRawAsync(DbMaintenanceScript.DropConstraintScript);
    }

    await new EnsureData(dbContext).RunAsync();

    Console.WriteLine("DbMigration Complete...");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex);
    Console.ResetColor();
}
