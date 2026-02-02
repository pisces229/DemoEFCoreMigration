using Microsoft.EntityFrameworkCore;
using Model.Definitions;
using Model.Entities;
using Model.JsonObjects;

namespace Test;

[TestClass]
public class TestApp : BaseTest
{
    [TestMethod(DisplayName = "Count")]
    public async Task Count()
    {
        var count = await _dbContext.AppTable.CountAsync();
        Assert.AreEqual(0, count);
    }

    [TestMethod(DisplayName = "Create")]
    public async Task Create()
    {
        //var dateTimeNow = DateTime.Now;
        _dbContext.AppTable.Add(new AppTable()
        {
            String = Guid.NewGuid().ToString(),
            Int = -1,
            //DateTime = dateTimeNow,
            //DateTime = DateTime.UtcNow,
            //DateTimeOffset = DateTimeOffset.Now,
            //DateTimeOffset = DateTimeOffset.UtcNow,
            StringJsonObjects = ["A", "B", "C"],
            ValueJsonObject = new ValueJsonObject(DateTime.UtcNow.AddDays(0), DateTime.UtcNow.AddDays(1)),
            ValueJsonObjects = [
                new (DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1)),
                new (DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(3))
            ],
        });
        await _dbContext.SaveChangesAsync();
    }

    [TestMethod(DisplayName = "Query")]
    public async Task Query()
    {
        var dateTimeNow = DateTime.Now;
        var datas = await _dbContext.AppTable
            //.Where(p => p.DateTime <= dateTimeNow)
            .ToListAsync();
        foreach (var data in datas)
        {
            Console.WriteLine(string.Join(", ", data.StringJsonObjects));
            if (data.DateTime.HasValue)
            {
                Console.WriteLine(data.DateTime.Value.Kind);
                Console.WriteLine(data.DateTime.Value.ToString());
                Console.WriteLine(data.DateTime.Value.ToLocalTime().ToString());
            }
        }
    }

    [TestMethod(DisplayName = "Query1")]
    public async Task Query1()
    {
        var query = _dbContext.AppTable.Where(p => p.Enum == DataType.First);
        Console.WriteLine(query.ToQueryString());
        await query.ToListAsync();
    }

    [TestMethod(DisplayName = "Query2")]
    public async Task Query2()
    {
        //var query = _demoContext.DemoTable.Where(p => p.DataType == DataType.First)
        //    .Select(s => s.StringValue);

        //Func<DemoTable, string> func = (t) => t.StringValue;
        //var query = _demoContext.DemoTable.Where(p => p.DataType == DataType.First)
        //    .Select(s => func(s));

        var dic = new Dictionary<string, Func<AppTable, string>>()
            {
                { "A", (t) => t.String }
            };
        var query = _dbContext.AppTable.Where(p => p.Enum == DataType.First)
            .Select(s => dic["A"](s));

        Console.WriteLine(query.ToQueryString());
        var list = await query.ToListAsync();
        Console.WriteLine(list);
    }

    [TestMethod(DisplayName = "Json")]
    public async Task Json()
    {
        var appTables = await _dbContext.AppTable.ToListAsync();
        foreach (var appTable in appTables)
        {
            if (appTable.ValueJsonObject != null)
            {
                Console.WriteLine($"DateValueObject.Start:{appTable.ValueJsonObject.StartDate}");
                Console.WriteLine($"DateValueObject.End:{appTable.ValueJsonObject.EndDate}");
            }
            foreach (var dateValueObject in appTable.ValueJsonObjects)
            {
                Console.WriteLine($"DateValueObjects.Start:{dateValueObject.StartDate}");
                Console.WriteLine($"DateValueObjects.End:{dateValueObject.EndDate}");
            }
        }
    }

}
