using Microsoft.EntityFrameworkCore;

namespace Test;

[TestClass]
public class TestHuman : BaseTest
{
    [TestMethod(DisplayName = "QueryHumanHead")]
    public async Task QueryHumanHead()
    {
        await _dbContext.HumanHead
            .Include(e => e.HumanBody)
            .OrderBy(e => e.HumanBody.Id)
            .ThenBy(e => e.Id)
            .ToListAsync();
    }

    [TestMethod(DisplayName = "QueryHumanBody")]
    public async Task QueryHumanBody()
    {
        await _dbContext.HumanBody
            .Include(e => e.HumanLimbs)
            .OrderBy(e => e.Id)
            .ToListAsync();
    }

    [TestMethod(DisplayName = "QueryHumanLimb")]
    public async Task QueryHumanLimb()
    {
        await _dbContext.HumanLimb
            .Include(e => e.HumanBody)
            .OrderBy(e => e.HumanBody.Id)
            .ThenBy(e => e.Id)
            .ToListAsync();
    }
}
