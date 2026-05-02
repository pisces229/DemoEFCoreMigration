using Model.JsonObjects;
using Model.JsonPayloads;
using System.Text.Json;

using Microsoft.Extensions.Logging;
namespace IntegrationTest;

/// <summary>
/// AppTable 各種欄位型別(基本型別、JSON 字串、自訂 JsonObject、RowVersion 等)的持久化驗證。
/// </summary>
[TestClass]
public class TestApp : BaseTest
{
    public override async Task TestCleanup()
    {
        var cancellationToken = default(CancellationToken);
        await _dbContext.AppTable.ExecuteDeleteAsync(cancellationToken);
        await base.TestCleanup();
    }

    [TestMethod]
    public async Task Count()
    {
        var cancellationToken = default(CancellationToken);
        var count = await _dbContext.AppTable.CountAsync(cancellationToken);
        Assert.AreEqual(0, count);
    }

    [TestMethod]
    public async Task Create()
    {
        var cancellationToken = default(CancellationToken);
        //var dateTimeNow = DateTime.Now;
        _dbContext.AppTable.Add(new AppTable()
        {
            String = Guid.CreateVersion7().ToString(),
            Int = 1,
            //DateTime = dateTimeNow,
            //DateTime = DateTime.UtcNow,
            //DateTimeOffset = DateTimeOffset.Now,
            //DateTimeOffset = DateTimeOffset.UtcNow,
            AnyJsonString = JsonSerializer.Serialize(new AnyJsonPayload("Name")),
            StringArray = ["A", "B", "C"],
            GuidArray = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()],
            ValueJsonObject = new ValueJsonObject(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(0)),
            ValueJsonObjects = [
                new (DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(-2)),
                new (DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-3))
            ],
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [TestMethod]
    public async Task Query()
    {
        var cancellationToken = default(CancellationToken);
        var dateTimeNow = DateTime.UtcNow;
        var datas = await _dbContext.AppTable
            //.Where(p => p.DateTime <= dateTimeNow)
            // jsonb gin index
            //.Where(a => EF.Functions.JsonContains(a.StringArray, "[\"A\"]"))
            // text array gin index
            .Where(a => a.StringArray.Contains("A"))
            // jsonb gin index
            .Where(p => p.ValueJsonObject!.StartDate <= dateTimeNow)
            .ToListAsync(cancellationToken);
        foreach (var data in datas)
        {
            _logger.LogInformation("{Message}", string.Join(", ", data.StringArray));
            if (data.DateTime.HasValue)
            {
                _logger.LogInformation("{Message}", data.DateTime.Value.Kind);
                _logger.LogInformation("{Message}", data.DateTime.Value.ToString());
                _logger.LogInformation("{Message}", data.DateTime.Value.ToLocalTime().ToString());
            }
        }
    }

    [TestMethod]
    public async Task Query1()
    {
        var cancellationToken = default(CancellationToken);
        var query = _dbContext.AppTable.Where(p => p.Enum == DataType.First);
        _logger.LogInformation("{Message}", query.ToQueryString());
        await query.ToListAsync(cancellationToken);
    }

    [TestMethod]
    public async Task Query2()
    {
        var cancellationToken = default(CancellationToken);
        //var query = _demoContext.DemoTable.Where(p => p.DataType == DataType.First)
        //    .Select(s => s.StringValue);

        //Func<DemoTable, string> func = (t) => t.StringValue;
        //var query = _demoContext.DemoTable.Where(p => p.DataType == DataType.First)
        //    .Select(s => func(s));

        var dic = new Dictionary<string, Func<AppTable, string?>>()
            {
                { "A", (t) => t.String }
            };
        var query = _dbContext.AppTable.Where(p => p.Enum == DataType.First)
            //.Where(e => e.Id == Guid.Empty)
            .Select(s => dic["A"](s));

        _logger.LogInformation("{Message}", query.ToQueryString());
        var list = await query.ToListAsync(cancellationToken);
        _logger.LogInformation("{Message}", list);
    }

    [TestMethod]
    public async Task Json()
    {
        var cancellationToken = default(CancellationToken);
        var appTables = await _dbContext.AppTable.ToListAsync(cancellationToken);
        foreach (var appTable in appTables)
        {
            if (appTable.ValueJsonObject != null)
            {
                _logger.LogInformation("{Message}", $"DateValueObject.Start:{appTable.ValueJsonObject.StartDate}");
                _logger.LogInformation("{Message}", $"DateValueObject.End:{appTable.ValueJsonObject.EndDate}");
            }
            foreach (var dateValueObject in appTable.ValueJsonObjects)
            {
                _logger.LogInformation("{Message}", $"DateValueObjects.Start:{dateValueObject.StartDate}");
                _logger.LogInformation("{Message}", $"DateValueObjects.End:{dateValueObject.EndDate}");
            }
        }
    }

}
