namespace IntegrationTest;

/// <summary>
/// PostgreSQL view (<c>view</c>) 透過 EF Core <c>ToView</c> 對應的查詢驗證,
/// 含直接 ToList 與 AnimalCat 透過導覽屬性 Include 取得 view 結果。
/// </summary>
[TestClass]
public class TestDbView : BaseTest
{
    [TestMethod]
    public async Task ViewResult()
    {
        await SeedData.SeedAsync(_dbContext);
        await _dbContext.ViewResult.ToListAsync();
    }

    [TestMethod]
    public async Task JoinViewResult()
    {
        await SeedData.SeedAsync(_dbContext);
        await _dbContext.AnimalCat.Include(e => e.ViewResult).ToListAsync();
    }
}
