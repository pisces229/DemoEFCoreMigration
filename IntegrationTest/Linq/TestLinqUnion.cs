namespace IntegrationTest;

/// <summary>
/// LINQ <c>Union</c> 翻譯為 SQL <c>UNION</c>(去重)的查詢驗證。
/// </summary>
[TestClass]
public class TestLinqUnion : BaseTest
{
    [TestMethod]
    public async Task UnionSelect()
    {
        var q = _dbContext.HumanHead;
        var r = await q.Where(w => w.Color == Color.Red)
            .Union(q.Where(w => w.Color == Color.Green))
            .Union(q.Where(w => w.Color == Color.Yellow))
            .ToListAsync();
    }
}
