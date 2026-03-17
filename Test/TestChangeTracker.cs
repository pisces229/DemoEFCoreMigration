using Microsoft.EntityFrameworkCore;
using Model.Definitions;
using Model.Entities;
using System.Diagnostics;

namespace Test;

[TestClass]
public class TestChangeTracker : BaseTest
{

    [TestMethod(DisplayName = "ChangeTracker")]
    public async Task ChangeTracker()
    {
        await _dbContext.HumanBody.FirstAsync();
        Console.WriteLine($"..........");
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            Console.WriteLine($"Entity.State:{entity.State}");
            Console.WriteLine($"entity.Entity.Remark:{entity.Entity.Remark}");
            entity.Entity.Remark = Guid.NewGuid().ToString();
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            Console.WriteLine($"Entity.State:{entity.State}");
            Console.WriteLine($"entity.Entity.Remark:{entity.Entity.Remark}");
            entity.State = EntityState.Unchanged;
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            Console.WriteLine($"Entity.State:{entity.State}");
            Console.WriteLine($"entity.Entity.Remark:{entity.Entity.Remark}");
        }
        Console.WriteLine($"..........");
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            Console.WriteLine($"Entity.State:{entity.State}");
            Console.WriteLine($"entity.Entity.Remark:{entity.Entity.Remark}");
            entity.Entity.Remark = Guid.NewGuid().ToString();
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            Console.WriteLine($"Entity.State:{entity.State}");
            Console.WriteLine($"entity.Entity.Remark:{entity.Entity.Remark}");
            await entity.ReloadAsync();
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            Console.WriteLine($"Entity.State:{entity.State}");
            Console.WriteLine($"entity.Entity.Remark:{entity.Entity.Remark}");
        }
    }

    [TestMethod(DisplayName = "AutoDetectChangesEnabled")]
    public async Task AutoDetectChangesEnabled()
    {
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        {
            var entity = await _dbContext.HumanBody.FirstAsync();
            entity.Remark = Guid.NewGuid().ToString();
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            Console.WriteLine($"Entity.State:{entity.State}");
            Console.WriteLine($"entity.Entity.Remark:{entity.Entity.Remark}");
        }
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            Console.WriteLine($"Entity.State:{entity.State}");
            Console.WriteLine($"entity.Entity.Remark:{entity.Entity.Remark}");
        }
    }

    [TestMethod(DisplayName = "AutoDetectChangesEnabledTruePerformance")]
    public Task AutoDetectChangesEnabledTruePerformance()
    {
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < Int16.MaxValue; i++)
        {
            _dbContext.HumanBody.Add(new HumanBody()
            {
                Ulid = "Add" + i,
                Weight = DateTime.Now.Millisecond,
                Color = Color.Red,
                CheckDate = DateTime.Now,
                Remark = Guid.NewGuid().ToString(),
            });
        }
        stopWatch.Stop();
        Console.WriteLine(stopWatch.ElapsedMilliseconds);
        return Task.CompletedTask;
    }

    [TestMethod(DisplayName = "AutoDetectChangesEnabledFalsePerformance")]
    public Task AutoDetectChangesEnabledFalsePerformance()
    {
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < Int16.MaxValue; i++)
        {
            _dbContext.HumanBody.Add(new HumanBody()
            {
                Ulid = "Add" + i,
                Weight = DateTime.Now.Millisecond,
                Color = Color.Red,
                CheckDate = DateTime.Now,
                Remark = Guid.NewGuid().ToString(),
            });
        }
        stopWatch.Stop();
        Console.WriteLine(stopWatch.ElapsedMilliseconds);
        return Task.CompletedTask;
    }

    [TestMethod(DisplayName = "LazyLoadingEnabledTrue")]
    public async Task LazyLoadingEnabledTrue()
    {
        _dbContext.ChangeTracker.LazyLoadingEnabled = true;
        var humanBody = await _dbContext.HumanBody.FirstAsync();
        //  (Explicit loading)
        //_demoContext.Entry(humanBody).Reference(p => p.HumanLimbs).Load();
        //_demoContext.Entry(humanBody).Collection(p => p.HumanLimbs).Load();
        // (Lazy loading)
        humanBody.HumanLimbs?.ToList();
        Console.WriteLine(humanBody.HumanLimbs?.Count);
    }

    [TestMethod(DisplayName = "LazyLoadingEnabledFalse")]
    public async Task LazyLoadingEnabledFalse()
    {
        _dbContext.ChangeTracker.LazyLoadingEnabled = false;
        var humanBody = await _dbContext.HumanBody.FirstAsync();
        //  (Explicit loading)
        //_demoContext.Entry(humanBody).Reference(p => p.HumanLimbs).Load();
        //_demoContext.Entry(humanBody).Collection(p => p.HumanLimbs).Load();
        // (Lazy loading)
        humanBody.HumanLimbs?.ToList();
        Console.WriteLine(humanBody.HumanLimbs?.Count);
    }

    [TestMethod(DisplayName = "Reload")]
    public async Task Reload()
    {
        var humanBody = await _dbContext.HumanBody.FirstAsync();
        Console.WriteLine(humanBody.Remark);
        Console.WriteLine(_dbContext.Entry(humanBody).State);
        humanBody.Remark = Guid.NewGuid().ToString();
        Console.WriteLine(humanBody.Remark);
        Console.WriteLine(_dbContext.Entry(humanBody).State);
        await _dbContext.Entry(humanBody).ReloadAsync();
        Console.WriteLine(humanBody.Remark);
        Console.WriteLine(_dbContext.Entry(humanBody).State);
    }

}
