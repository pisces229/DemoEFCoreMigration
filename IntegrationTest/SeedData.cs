namespace IntegrationTest;

/// <summary>
/// 共用 seed 資料：由各測試在需要時自行呼叫。
/// </summary>
public static class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        dbContext.AnimalCat.AddRange(
            new AnimalCat { Name = "Whiskers", BirthDate = DateTime.UtcNow, WhiskerLength = 7.5, Flag = Flag.First | Flag.Second },
            new AnimalCat { Name = "Mittens", BirthDate = DateTime.UtcNow, WhiskerLength = 6, Flag = Flag.First | Flag.Third });

        var body = new HumanBody
        {
            Ulid = Guid.CreateVersion7().ToString("N"),
            Weight = 60,
            Color = Color.Green,
            CheckDate = DateTime.UtcNow,
            HumanLimbs =
            [
                new HumanLimb { Ulid = Guid.CreateVersion7().ToString("N"), Weight = 8, Color = Color.Red, CheckDate = DateTime.UtcNow },
            ],
        };
        dbContext.HumanHead.Add(new HumanHead
        {
            Ulid = Guid.CreateVersion7().ToString("N"),
            Weight = 5,
            Color = Color.Yellow,
            CheckDate = DateTime.UtcNow,
            HumanBody = body,
        });

        await dbContext.SaveChangesAsync();
    }
}
