namespace IntegrationTest;

/// <summary>
/// 對照 LINQ Query syntax(<c>from … select</c>)與 Method syntax(Fluent API)
/// 在 EF Core 下產生的同質 SQL。
/// </summary>
[TestClass]
public class TestSyntax : BaseTest
{
    [TestMethod]
    public async Task QuerySyntaxInclude()
    {
        await (from e in _dbContext.HumanBody.Include(e => e.HumanLimbs)
               orderby e.Id
               select e).ToListAsync();
    }

    [TestMethod]
    public async Task QuerySyntaxJoin()
    {
        await (from body in _dbContext.HumanBody
               join limb in _dbContext.HumanLimb on (Guid?)body.Id equals limb.BodyId
               orderby body.Id
               select new
               {
                   Body = body,
                   Limb = limb
               }).ToListAsync();
    }

    [TestMethod]
    public async Task MethodSyntax()
    {
        await _dbContext.HumanHead
            .Include(e => e.HumanBody)
            .OrderBy(e => e.HumanBody.Id)
            .ThenBy(e => e.Id)
            .ToListAsync();
    }
}
