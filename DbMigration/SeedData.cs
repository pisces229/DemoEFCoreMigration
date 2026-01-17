using Microsoft.EntityFrameworkCore;
using Model;
using Model.Definitions;
using Model.Entities;

namespace DbMigration;

public class SeedData(ApplicationDbContext _dbContext)
{
    public async Task RunAsync()
    {
        Console.WriteLine("Seed Data...");
        await CreateAnimalCatsAsync();
    }

    private async Task CreateAnimalCatsAsync()
    {
        if (!await _dbContext.AnimalCat.AnyAsync())
        {
            var cats = new List<AnimalCat>
            {
                new() { Name = "Whiskers", BirthDate = DateTime.UtcNow, WhiskerLength = 7.5,  Flag = Flag.First | Flag.Second },
                new() { Name = "Mittens", BirthDate = DateTime.UtcNow, WhiskerLength = 6, Flag = Flag.First | Flag.Third },
            };
            await _dbContext.AnimalCat.AddRangeAsync(cats);
            await _dbContext.SaveChangesAsync();
        }
    }
}
