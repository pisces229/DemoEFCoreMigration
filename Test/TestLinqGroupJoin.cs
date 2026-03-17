using Microsoft.EntityFrameworkCore;

namespace Test;

[TestClass]
public class TestLinqGroupJoin : BaseTest
{
    [TestMethod(DisplayName = "GroupJoinSelect")]
    public async Task GroupJoinSelect()
    {
        var query = _dbContext.HumanBody;
        var r = await query.GroupJoin(_dbContext.HumanLimb,
            t1 => t1.Ulid,
            t2 => t2.BodyId,
            (t1, t2Group) => new { t1, t2Group = t2Group.FirstOrDefault() })
            .Select(s => new
            {
                Id1 = s.t1.Ulid,
                Id2 = s.t2Group == null ? "" : s.t2Group.Ulid
            })
            // other condition
            .ToListAsync();
    }

    [TestMethod("GroupJoinSelectMany")]
    public async Task GroupJoinSelectMany()
    {
        var query = _dbContext.HumanBody;
        var r = await query.GroupJoin(_dbContext.HumanLimb,
            t1 => t1.Ulid,
            t2 => t2.BodyId,
            (t1, t2Group) => new { t1, t2Group })
            .SelectMany(s => s.t2Group.DefaultIfEmpty(),
            (t1, t2) => new
            {
                Id1 = t1.t1.Ulid,
                Id2 = t2 == null ? "" : t2.Ulid
            })
            // other condition
            .ToListAsync();
    }
}
