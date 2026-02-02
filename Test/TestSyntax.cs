using Microsoft.EntityFrameworkCore;

namespace Test;

[TestClass]
public class TestSyntax : BaseTest
{
    [TestMethod(DisplayName = "Query Syntax Include")]
    public async Task QuerySyntaxInclude()
    {
        await (from e in _dbContext.HumanBody.Include(e => e.HumanLimbs)
               orderby e.Id
               select e).ToListAsync();
    }

    [TestMethod(DisplayName = "Query Syntax Join")]
    public async Task QuerySyntaxJoin()
    {
        await (from body in _dbContext.HumanBody
               join limb in _dbContext.HumanLimb on body.Ulid equals limb.BodyId
               orderby body.Id
               select new
               {
                   Body = body,
                   Limb = limb
               }).ToListAsync();
    }

    [TestMethod(DisplayName = "Method Syntax/Fluent API")]
    public async Task MethodSyntax()
    {
        await _dbContext.HumanHead
            .Include(e => e.HumanBody)
            .OrderBy(e => e.HumanBody.Id)
            .ThenBy(e => e.Id)
            .ToListAsync();
    }
}
