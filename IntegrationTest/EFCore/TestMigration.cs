namespace IntegrationTest;

/// <summary>
/// 容器資料庫的健全性檢查:可連線、所有 EF Core migration 已套用無 pending。
/// </summary>
[TestClass]
public class TestMigration : BaseTest
{
    [TestMethod]
    public async Task CanConnect()
    {
        var canConnect = await _dbContext.Database.CanConnectAsync();
        Assert.IsTrue(canConnect);
    }

    [TestMethod]
    public async Task AllMigrationsApplied()
    {
        var pending = await _dbContext.Database.GetPendingMigrationsAsync();
        CollectionAssert.AreEqual(Array.Empty<string>(), pending.ToArray());
    }
}
