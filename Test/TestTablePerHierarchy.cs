using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Newtonsoft.Json;

namespace Test;

[TestClass]
public class TestTablePerHierarchy : BaseTest
{
    [TestMethod(DisplayName = "Query")]
    public async Task Query()
    {
        var t = await _dbContext.TablePerHierarchyTables.ToListAsync();
        Console.WriteLine(JsonConvert.SerializeObject(t, Formatting.Indented));
        var t1 = await _dbContext.TablePerHierarchyTables.OfType<TablePerHierarchy1>().ToListAsync();
        Console.WriteLine(JsonConvert.SerializeObject(t1, Formatting.Indented));
        var t2 = await _dbContext.TablePerHierarchyTables.OfType<TablePerHierarchy2>().ToListAsync();
        Console.WriteLine(JsonConvert.SerializeObject(t2, Formatting.Indented));

        Assert.AreEqual(t.Count, t1.Count + t2.Count);
    }

    [TestMethod(DisplayName = "Create")]
    public async Task Create()
    {
        _dbContext.TablePerHierarchyTables.Add(new TablePerHierarchy1()
        {
            Name = "1",
            Name1 = "11",
        });
        _dbContext.TablePerHierarchyTables.Add(new TablePerHierarchy2()
        {
            Name = "2",
            Name2 = "22",
        });
        await _dbContext.SaveChangesAsync();
        await Query();
    }
}
