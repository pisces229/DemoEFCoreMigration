using Microsoft.EntityFrameworkCore;

namespace Test;

[TestClass]
public class TestLinqJoin : BaseTest
{
    [TestMethod(DisplayName = "JoinSelect")]
    public async Task JoinSelect()
    {
        var query = _dbContext.HumanHead;
        var r = await query.Join(_dbContext.HumanBody,
            t1 => t1.Ulid,
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
