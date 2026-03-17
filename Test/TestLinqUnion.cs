using Microsoft.EntityFrameworkCore;
using Model.Definitions;

namespace Test;

[TestClass]
public class TestLinqUnion : BaseTest
{
    [TestMethod(DisplayName = "UnionSelect")]
    public async Task UnionSelect()
    {
        var q = _dbContext.HumanHead;
        var r = await q.Where(w => w.Color == Color.Red)
            .Union(q.Where(w => w.Color == Color.Green))
            .Union(q.Where(w => w.Color == Color.Yellow))
            .ToListAsync();
    }
}
