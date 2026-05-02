namespace IntegrationTest;

/// <summary>
/// LINQ <c>Join</c> 翻譯為 SQL INNER JOIN 並投影成匿名型別的查詢測試。
/// </summary>
[TestClass]
public class TestLinqJoin : BaseTest
{
    [TestMethod]
    public async Task JoinSelect()
    {
        var query = _dbContext.HumanHead;
        var r = await query.Join(_dbContext.HumanBody,
            t1 => (Guid?)t1.Id,
            t2 => t2.HeadId,
            (t1, t2) => new { t1, t2 })
            .Select(s => new
            {
                Id1 = s.t1.Ulid,
                Id2 = s.t2.Ulid,
            })
            // other condition
            .ToListAsync();
    }
}
