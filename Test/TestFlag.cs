using Microsoft.EntityFrameworkCore;
using Model.Definitions;

namespace Test;

[TestClass]
public class TestFlag : BaseTest
{
    [TestMethod(DisplayName = "Query")]
    public async Task Query()
    {
        await _dbContext.AnimalCat
            .Where(p => p.Flag.HasFlag(Flag.First | Flag.Fourth))
            .ToListAsync();
    }
}
