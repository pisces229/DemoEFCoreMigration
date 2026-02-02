using Model;

namespace DbMigration;

public class EnsureData(ApplicationDbContext _dbContext)
{
    public async Task RunAsync()
    {
        Console.WriteLine("Ensure Data...");
        await _dbContext.SaveChangesAsync();
    }
}
