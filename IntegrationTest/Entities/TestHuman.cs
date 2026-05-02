namespace IntegrationTest;

/// <summary>
/// HumanHead → HumanBody → HumanLimb 三層關係(1:1 + 1:N)的寫入與
/// Include / ThenInclude 導覽屬性查詢驗證。
/// </summary>
[TestClass]
public class TestHuman : BaseTest
{
    [TestMethod]
    public async Task InsertAndQueryGraph()
    {
        var head = new HumanHead
        {
            Ulid = Guid.CreateVersion7().ToString("N"),
            Weight = 5,
            Color = Color.Yellow,
            CheckDate = DateTime.UtcNow,
            HumanBody = new HumanBody
            {
                Ulid = Guid.CreateVersion7().ToString("N"),
                Weight = 60,
                Color = Color.Green,
                CheckDate = DateTime.UtcNow,
                HumanLimbs = new List<HumanLimb>
                {
                    new() { Ulid = Guid.CreateVersion7().ToString("N"), Weight = 8, Color = Color.Red, CheckDate = DateTime.UtcNow },
                    new() { Ulid = Guid.CreateVersion7().ToString("N"), Weight = 9, Color = Color.Red, CheckDate = DateTime.UtcNow },
                },
            },
        };

        _dbContext.HumanHead.Add(head);
        var affected = await _dbContext.SaveChangesAsync();
        Assert.AreEqual(4, affected);

        await using var verifyContext = CreateDbContext();
        var loaded = await verifyContext.HumanHead
            .Include(e => e.HumanBody)
                .ThenInclude(b => b.HumanLimbs)
            .SingleAsync(e => e.Id == head.Id);

        Assert.IsNotNull(loaded.HumanBody);
        Assert.AreEqual(60, loaded.HumanBody.Weight);
        Assert.HasCount(2, loaded.HumanBody.HumanLimbs!);
    }

    [TestMethod]
    public async Task QueryHumanHead()
    {
        await _dbContext.HumanHead
            .Include(e => e.HumanBody)
            .OrderBy(e => e.HumanBody.Id)
            .ThenBy(e => e.Id)
            .ToListAsync();
    }

    [TestMethod]
    public async Task QueryHumanBody()
    {
        await _dbContext.HumanBody
            .Include(e => e.HumanLimbs)
            .OrderBy(e => e.Id)
            .ToListAsync();
    }

    [TestMethod]
    public async Task QueryHumanLimb()
    {
        await _dbContext.HumanLimb
            .Include(e => e.HumanBody)
            .OrderBy(e => e.HumanBody!.Id)
            .ThenBy(e => e.Id)
            .ToListAsync();
    }
}
