namespace IntegrationTest;

/// <summary>
/// LINQ <c>Concat</c> 在 EF Core 下翻譯為 SQL <c>UNION ALL</c> 的查詢驗證。
/// </summary>
[TestClass]
public class TestLinqConcat : BaseTest
{
    [TestMethod]
    public async Task ConcatSelect()
    {
        var q = _dbContext.HumanHead;
        var r = await q.Where(w => w.Color == Color.Red)
            .Concat(q.Where(w => w.Color == Color.Green))
            .Concat(q.Where(w => w.Color == Color.Yellow))
            .ToListAsync();
    }
}
