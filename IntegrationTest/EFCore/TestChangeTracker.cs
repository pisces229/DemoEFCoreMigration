using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace IntegrationTest;

/// <summary>
/// EF Core <see cref="ChangeTracker"/> 行為觀察:Entity State 切換、
/// AutoDetectChangesEnabled 啟/停下的效能差異、Lazy Loading 與 Reload。
/// </summary>
[TestClass]
public class TestChangeTracker : BaseTest
{

    [TestMethod]
    public async Task ChangeTracker()
    {
        await SeedData.SeedAsync(_dbContext);
        await _dbContext.HumanBody.FirstAsync();
        _logger.LogInformation("{Message}", $"..........");
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            _logger.LogInformation("{Message}", $"Entity.State:{entity.State}");
            _logger.LogInformation("{Message}", $"entity.Entity.Remark:{entity.Entity.Remark}");
            entity.Entity.Remark = Guid.CreateVersion7().ToString();
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            _logger.LogInformation("{Message}", $"Entity.State:{entity.State}");
            _logger.LogInformation("{Message}", $"entity.Entity.Remark:{entity.Entity.Remark}");
            entity.State = EntityState.Unchanged;
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            _logger.LogInformation("{Message}", $"Entity.State:{entity.State}");
            _logger.LogInformation("{Message}", $"entity.Entity.Remark:{entity.Entity.Remark}");
        }
        _logger.LogInformation("{Message}", $"..........");
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            _logger.LogInformation("{Message}", $"Entity.State:{entity.State}");
            _logger.LogInformation("{Message}", $"entity.Entity.Remark:{entity.Entity.Remark}");
            entity.Entity.Remark = Guid.CreateVersion7().ToString();
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            _logger.LogInformation("{Message}", $"Entity.State:{entity.State}");
            _logger.LogInformation("{Message}", $"entity.Entity.Remark:{entity.Entity.Remark}");
            await entity.ReloadAsync();
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            _logger.LogInformation("{Message}", $"Entity.State:{entity.State}");
            _logger.LogInformation("{Message}", $"entity.Entity.Remark:{entity.Entity.Remark}");
        }
    }

    [TestMethod]
    public async Task AutoDetectChangesEnabled()
    {
        await SeedData.SeedAsync(_dbContext);
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        {
            var entity = await _dbContext.HumanBody.FirstAsync();
            entity.Remark = Guid.CreateVersion7().ToString();
        }
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            _logger.LogInformation("{Message}", $"Entity.State:{entity.State}");
            _logger.LogInformation("{Message}", $"entity.Entity.Remark:{entity.Entity.Remark}");
        }
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        foreach (var entity in _dbContext.ChangeTracker.Entries<HumanBody>())
        {
            _logger.LogInformation("{Message}", $"Entity.State:{entity.State}");
            _logger.LogInformation("{Message}", $"entity.Entity.Remark:{entity.Entity.Remark}");
        }
    }

    [TestMethod]
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
                Remark = Guid.CreateVersion7().ToString(),
            });
        }
        stopWatch.Stop();
        _logger.LogInformation("{Message}", stopWatch.ElapsedMilliseconds);
        return Task.CompletedTask;
    }

    [TestMethod]
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
                Remark = Guid.CreateVersion7().ToString(),
            });
        }
        stopWatch.Stop();
        _logger.LogInformation("{Message}", stopWatch.ElapsedMilliseconds);
        return Task.CompletedTask;
    }

    [TestMethod]
    public async Task LazyLoadingEnabledTrue()
    {
        await SeedData.SeedAsync(_dbContext);
        _dbContext.ChangeTracker.LazyLoadingEnabled = true;
        var humanBody = await _dbContext.HumanBody.FirstAsync();
        //  (Explicit loading)
        //_demoContext.Entry(humanBody).Reference(p => p.HumanLimbs).Load();
        //_demoContext.Entry(humanBody).Collection(p => p.HumanLimbs).Load();
        // (Lazy loading)
        humanBody.HumanLimbs?.ToList();
        _logger.LogInformation("{Message}", humanBody.HumanLimbs?.Count);
    }

    [TestMethod]
    public async Task LazyLoadingEnabledFalse()
    {
        await SeedData.SeedAsync(_dbContext);
        _dbContext.ChangeTracker.LazyLoadingEnabled = false;
        var humanBody = await _dbContext.HumanBody.FirstAsync();
        //  (Explicit loading)
        //_demoContext.Entry(humanBody).Reference(p => p.HumanLimbs).Load();
        //_demoContext.Entry(humanBody).Collection(p => p.HumanLimbs).Load();
        // (Lazy loading)
        humanBody.HumanLimbs?.ToList();
        _logger.LogInformation("{Message}", humanBody.HumanLimbs?.Count);
    }

    [TestMethod]
    public async Task Reload()
    {
        await SeedData.SeedAsync(_dbContext);
        var humanBody = await _dbContext.HumanBody.FirstAsync();
        _logger.LogInformation("{Message}", humanBody.Remark);
        _logger.LogInformation("{Message}", _dbContext.Entry(humanBody).State);
        humanBody.Remark = Guid.CreateVersion7().ToString();
        _logger.LogInformation("{Message}", humanBody.Remark);
        _logger.LogInformation("{Message}", _dbContext.Entry(humanBody).State);
        await _dbContext.Entry(humanBody).ReloadAsync();
        _logger.LogInformation("{Message}", humanBody.Remark);
        _logger.LogInformation("{Message}", _dbContext.Entry(humanBody).State);
    }

}
