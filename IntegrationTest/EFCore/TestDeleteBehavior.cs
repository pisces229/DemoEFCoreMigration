namespace IntegrationTest;

/// <summary>
/// 7 種 <c>DeleteBehavior</c> 命名的測試,在當前 entity 配置
/// (<c>HumanBody</c>→<c>HumanLimb</c>: Restrict + nullable FK)下實際展現的
/// EF Core client-side 行為:parent 被刪除、child 的 BodyId 自動 SET NULL,
/// 等同 <c>ClientSetNull</c>。
/// </summary>
[TestClass]
public class TestDeleteBehavior : BaseTest
{
    private static (HumanBody body, HumanLimb limb) Build()
    {
        var body = new HumanBody { Ulid = "1", Weight = 0, Color = Color.Red, CheckDate = DateTime.UtcNow };
        var limb = new HumanLimb { Ulid = "1", Weight = 0, Color = Color.Red, CheckDate = DateTime.UtcNow, BodyId = body.Id };
        return (body, limb);
    }

    private async Task ArrangeAsync(HumanBody body, HumanLimb limb)
    {
        _dbContext.HumanBody.Add(body);
        _dbContext.HumanLimb.Add(limb);
        await _dbContext.SaveChangesAsync();
    }

    private async Task AssertParentRemovedChildOrphanedAsync(HumanBody body, HumanLimb limb)
    {
        Assert.IsFalse(await _dbContext.HumanBody.AnyAsync(e => e.Id == body.Id));
        Assert.IsTrue(await _dbContext.HumanLimb.AnyAsync(e => e.Id == limb.Id));
        Assert.IsNull(limb.BodyId);
    }

    [TestMethod]
    public async Task Cascade()
    {
        var (body, limb) = Build();
        await ArrangeAsync(body, limb);

        _dbContext.HumanBody.Remove(body);
        await _dbContext.SaveChangesAsync();

        await AssertParentRemovedChildOrphanedAsync(body, limb);
    }

    [TestMethod]
    public async Task ClientCascade()
    {
        var (body, limb) = Build();
        await ArrangeAsync(body, limb);

        _dbContext.HumanBody.Remove(body);
        await _dbContext.SaveChangesAsync();

        await AssertParentRemovedChildOrphanedAsync(body, limb);
    }

    [TestMethod]
    public async Task Restrict()
    {
        var (body, limb) = Build();
        await ArrangeAsync(body, limb);

        _dbContext.HumanBody.Remove(body);
        await _dbContext.SaveChangesAsync();

        await AssertParentRemovedChildOrphanedAsync(body, limb);
    }

    [TestMethod]
    public async Task SetNull()
    {
        var (body, limb) = Build();
        await ArrangeAsync(body, limb);

        _dbContext.HumanBody.Remove(body);
        await _dbContext.SaveChangesAsync();

        await AssertParentRemovedChildOrphanedAsync(body, limb);
    }

    [TestMethod]
    public async Task ClientSetNull()
    {
        var (body, limb) = Build();
        await ArrangeAsync(body, limb);

        _dbContext.HumanBody.Remove(body);
        await _dbContext.SaveChangesAsync();

        await AssertParentRemovedChildOrphanedAsync(body, limb);
    }

    [TestMethod]
    public async Task NoAction()
    {
        var (body, limb) = Build();
        await ArrangeAsync(body, limb);

        _dbContext.HumanBody.Remove(body);
        await _dbContext.SaveChangesAsync();

        await AssertParentRemovedChildOrphanedAsync(body, limb);
    }

    [TestMethod]
    public async Task ClientNoAction()
    {
        var (body, limb) = Build();
        await ArrangeAsync(body, limb);

        _dbContext.HumanBody.Remove(body);
        await _dbContext.SaveChangesAsync();

        await AssertParentRemovedChildOrphanedAsync(body, limb);
    }
}
