using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;
using Model.Definitions;
using Model.Entities;

namespace Test;

[TestClass]
public class TestDeleteBehavior
{
    private ApplicationDbContext _demoContext { get; set; } = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        Console.WriteLine("TestInitialize");
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();
        var connectionString = configurationRoot.GetConnectionString("Demo");
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        dbContextOptionsBuilder.LogTo(message => Console.WriteLine(message), LogLevel.Information);
        dbContextOptionsBuilder.EnableSensitiveDataLogging();
        dbContextOptionsBuilder.UseSqlServer(connectionString);

        _demoContext = new ApplicationDbContext(dbContextOptionsBuilder.Options);
    }

    [TestMethod(DisplayName = "Cascade")]
    public async Task Cascade()
    {
        var id = "1";
        var humanBody = new HumanBody()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
        };
        var humanLimb = new HumanLimb()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
            BodyId = id,
        };

        await _demoContext.HumanBody.AddAsync(humanBody);
        await _demoContext.HumanLimb.AddAsync(humanLimb);
        await _demoContext.SaveChangesAsync();

        _demoContext.HumanBody.Remove(humanBody);
        await _demoContext.SaveChangesAsync();

        Assert.AreEqual(0, await _demoContext.HumanBody.CountAsync());
        Assert.AreEqual(0, await _demoContext.HumanLimb.CountAsync());
    }

    [TestMethod(DisplayName = "ClientCascade")]
    public async Task ClientCascade()
    {
        var id = "1";
        var humanBody = new HumanBody()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
        };
        var humanLimb = new HumanLimb()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
            BodyId = id,
        };

        await _demoContext.HumanBody.AddAsync(humanBody);
        await _demoContext.HumanLimb.AddAsync(humanLimb);
        await _demoContext.SaveChangesAsync();

        _demoContext.HumanBody.Remove(humanBody);
        await _demoContext.SaveChangesAsync();

        Assert.AreEqual(0, await _demoContext.HumanBody.CountAsync());
        Assert.AreEqual(0, await _demoContext.HumanLimb.CountAsync());
    }

    [TestMethod(DisplayName = "Restrict")]
    public async Task Restrict()
    {
        var id = "1";
        var humanBody = new HumanBody()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
        };
        var humanLimb = new HumanLimb()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
            BodyId = id,
        };

        await _demoContext.HumanLimb.AddAsync(humanLimb);
        await _demoContext.HumanBody.AddAsync(humanBody);
        await _demoContext.SaveChangesAsync();

        _demoContext.HumanBody.Remove(humanBody);
        await _demoContext.SaveChangesAsync();

        Assert.AreEqual(0, await _demoContext.HumanBody.CountAsync());
        Assert.AreEqual(1, await _demoContext.HumanLimb.CountAsync());
        Assert.IsNull(humanLimb.BodyId);
    }

    [TestMethod(DisplayName = "SetNull")]
    public async Task SetNull()
    {
        var id = "1";
        var humanBody = new HumanBody()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
        };
        var humanLimb = new HumanLimb()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
            BodyId = id,
        };

        await _demoContext.HumanBody.AddAsync(humanBody);
        await _demoContext.HumanLimb.AddAsync(humanLimb);
        await _demoContext.SaveChangesAsync();

        _demoContext.HumanBody.Remove(humanBody);
        await _demoContext.SaveChangesAsync();

        Assert.AreEqual(0, await _demoContext.HumanBody.CountAsync());
        Assert.AreEqual(1, await _demoContext.HumanLimb.CountAsync());
        Assert.IsNull(humanLimb.BodyId);
    }

    [TestMethod(DisplayName = "ClientSetNull")]
    public async Task ClientSetNull()
    {
        var id = "1";
        var humanBody = new HumanBody()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
        };
        var humanLimb = new HumanLimb()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
            BodyId = id,
        };

        await _demoContext.HumanBody.AddAsync(humanBody);
        await _demoContext.HumanLimb.AddAsync(humanLimb);
        await _demoContext.SaveChangesAsync();

        _demoContext.HumanBody.Remove(humanBody);
        await _demoContext.SaveChangesAsync();

        Assert.AreEqual(0, await _demoContext.HumanBody.CountAsync());
        Assert.AreEqual(1, await _demoContext.HumanLimb.CountAsync());
        Assert.IsNull(humanLimb.BodyId);
    }

    [TestMethod(DisplayName = "NoAction")]
    public async Task NoAction()
    {
        var id = "1";
        var humanBody = new HumanBody()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
        };
        var humanLimb = new HumanLimb()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
            BodyId = id,
        };

        await _demoContext.HumanBody.AddAsync(humanBody);
        await _demoContext.HumanLimb.AddAsync(humanLimb);
        await _demoContext.SaveChangesAsync();

        _demoContext.HumanBody.Remove(humanBody);
        await _demoContext.SaveChangesAsync();

        Assert.AreEqual(0, await _demoContext.HumanBody.CountAsync());
        Assert.AreEqual(1, await _demoContext.HumanLimb.CountAsync());
        Assert.IsNull(humanLimb.BodyId);
    }

    [TestMethod(DisplayName = "ClientNoAction")]
    public async Task ClientNoAction()
    {
        var id = "1";
        var humanBody = new HumanBody()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
        };
        var humanLimb = new HumanLimb()
        {
            Ulid = id,
            Weight = 0,
            Color = Color.Red,
            CheckDate = DateTime.Now,
            BodyId = id,
        };

        await _demoContext.HumanBody.AddAsync(humanBody);
        await _demoContext.HumanLimb.AddAsync(humanLimb);
        await _demoContext.SaveChangesAsync();

        //humanLimb.BodyId = null;
        //_demoContext.HumanLimb.Update(humanLimb);
        _demoContext.HumanBody.Remove(humanBody);
        await _demoContext.SaveChangesAsync();

        Assert.AreEqual(0, await _demoContext.HumanBody.CountAsync());
        Assert.AreEqual(1, await _demoContext.HumanLimb.CountAsync());
        Assert.IsNull(humanLimb.BodyId);
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        Console.WriteLine("TestCleanup");
        //_demoContext.HumanLimb.RemoveRange(await _demoContext.HumanLimb.ToListAsync());
        //_demoContext.HumanHead.RemoveRange(await _demoContext.HumanHead.ToListAsync());
        //await _demoContext.SaveChangesAsync();
        await _demoContext.DisposeAsync();
    }

}
