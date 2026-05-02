namespace IntegrationTest;

/// <summary>
/// <c>[Flags]</c> enum 的 <c>HasFlag</c> 在 LINQ 中翻譯成 SQL bitwise (<c>&amp;</c>) 運算的查詢。
/// </summary>
[TestClass]
public class TestFlag : BaseTest
{
    [TestMethod]
    public async Task Query()
    {
        await SeedData.SeedAsync(_dbContext);
        await _dbContext.AnimalCat
            .Where(p => p.Flag.HasFlag(Flag.First | Flag.Fourth))
            .ToListAsync();
    }
}
