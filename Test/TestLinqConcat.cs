using Microsoft.EntityFrameworkCore;
using Model.Definitions;

namespace Test;

[TestClass]
public class TestLinqConcat : BaseTest
{
    [TestMethod(DisplayName = "ConcatSelect")]
    public async Task ConcatSelect()
    {
        var q = _dbContext.HumanHead;
        var r = await q.Where(w => w.Color == Color.Red)
            .Concat(q.Where(w => w.Color == Color.Green))
            .Concat(q.Where(w => w.Color == Color.Yellow))
            .ToListAsync();
    }
}
