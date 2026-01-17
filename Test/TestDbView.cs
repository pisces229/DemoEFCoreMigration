using Microsoft.EntityFrameworkCore;

namespace Test;

[TestClass]
public class TestDbView : BaseTest
{
    [TestMethod(DisplayName = "ViewResult")]
    public async Task ViewResult()
    {
        await _dbContext.ViewResult.ToListAsync();
    }

    [TestMethod(DisplayName = "JoinViewResult")]
    public async Task JoinViewResult()
    {
        await _dbContext.AnimalCat.Include(e => e.ViewResult).ToListAsync();
    }
}
