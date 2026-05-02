using Microsoft.Extensions.Logging;

namespace IntegrationTest;

/// <summary>
/// FamilyParent 與 FamilyChild1 / FamilyChild2 的 1:N 關聯寫入與導覽屬性查詢
/// (FamilyChild2 的集合屬性目前在 entity 上被註解,需透過 DbSet 直接寫入)。
/// </summary>
[TestClass]
public class TestFamily : BaseTest
{
    public override async Task TestCleanup()
    {
        var cancellationToken = default(CancellationToken);
        await _dbContext.FamilyChild1.ExecuteDeleteAsync(cancellationToken);
        await _dbContext.FamilyChild2.ExecuteDeleteAsync(cancellationToken);
        await _dbContext.FamilyParent.ExecuteDeleteAsync(cancellationToken);
        await base.TestCleanup();
    }

    [TestMethod]
    public async Task Write_Parent()
    {
        var cancellationToken = default(CancellationToken);

        var parent = new FamilyParent
        {
            Name = "Parent-1",
        };

        await _dbContext.FamilyParent.AddAsync(parent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Assert.AreNotEqual(Guid.Empty, parent.Id, "FamilyParent should have a generated Id after save.");
        _logger.LogInformation("{Message}", $"FamilyParent Id={parent.Id}, Name={parent.Name}");
    }

    [TestMethod]
    public async Task Write_Children()
    {
        var cancellationToken = default(CancellationToken);

        var parent = new FamilyParent
        {
            Name = "Parent-2",
        };
        await _dbContext.FamilyParent.AddAsync(parent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var child1 = new FamilyChild1
        {
            Name = "Child1-A",
            ParentId = parent.Id,
        };
        var child2 = new FamilyChild2
        {
            Name = "Child2-A",
            ParentId = parent.Id,
        };

        await _dbContext.FamilyChild1.AddAsync(child1, cancellationToken);
        await _dbContext.FamilyChild2.AddAsync(child2, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Assert.AreNotEqual(Guid.Empty, child1.Id, "FamilyChild1 should have a generated Id after save.");
        Assert.AreNotEqual(Guid.Empty, child2.Id, "FamilyChild2 should have a generated Id after save.");
        Assert.AreEqual(parent.Id, child1.ParentId);
        Assert.AreEqual(parent.Id, child2.ParentId);

        _logger.LogInformation("{Message}", $"FamilyChild1 Id={child1.Id}, Name={child1.Name}, ParentId={child1.ParentId}");
        _logger.LogInformation("{Message}", $"FamilyChild2 Id={child2.Id}, Name={child2.Name}, ParentId={child2.ParentId}");
    }

    [TestMethod]
    public async Task Write_WithNavigation()
    {
        var cancellationToken = default(CancellationToken);

        var parent = new FamilyParent
        {
            Name = "Parent-3",
            FamilyChild1 =
            [
                new FamilyChild1 { Name = "Child1-B1" },
                new FamilyChild1 { Name = "Child1-B2" },
            ],
            // FamilyChild2 =
            // [
            //     new FamilyChild2 { Name = "Child2-B1" },
            // ],
        };

        await _dbContext.FamilyParent.AddAsync(parent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Assert.HasCount(2, parent.FamilyChild1);
        // Assert.HasCount(1, parent.FamilyChild2);

        foreach (var c in parent.FamilyChild1)
            _logger.LogInformation("{Message}", $"FamilyChild1 Id={c.Id}, Name={c.Name}, ParentId={c.ParentId}");
        // foreach (var c in parent.FamilyChild2)
        //     Console.WriteLine($"FamilyChild2 Id={c.Id}, Name={c.Name}, ParentId={c.ParentId}");
    }

    [TestMethod]
    public async Task Read_Parent()
    {
        var cancellationToken = default(CancellationToken);

        // Seed
        var parent = new FamilyParent
        {
            Name = "Parent-Read",
        };
        await _dbContext.FamilyParent.AddAsync(parent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Read
        var found = await _dbContext.FamilyParent
            .Where(p => p.Id == parent.Id)
            .FirstOrDefaultAsync(cancellationToken);

        Assert.IsNotNull(found);
        Assert.AreEqual("Parent-Read", found.Name);
        _logger.LogInformation("{Message}", $"Read FamilyParent Id={found.Id}, Name={found.Name}");
    }

    [TestMethod]
    public async Task Read_ChildrenByParent()
    {
        var cancellationToken = default(CancellationToken);

        // Seed
        var parent = new FamilyParent
        {
            Name = "Parent-ReadChildren",
            FamilyChild1 =
            [
                new FamilyChild1 { Name = "Child1-R1" },
                new FamilyChild1 { Name = "Child1-R2" },
            ],
        };
        await _dbContext.FamilyParent.AddAsync(parent, cancellationToken);
        await _dbContext.FamilyChild2.AddRangeAsync(
            [
                new FamilyChild2 { Name = "Child2-R1", ParentId = parent.Id },
                new FamilyChild2 { Name = "Child2-R2", ParentId = parent.Id },
                new FamilyChild2 { Name = "Child2-R3", ParentId = parent.Id },
            ],
            cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Read child1 list
        var child1List = await _dbContext.FamilyChild1
            .Where(c => c.ParentId == parent.Id)
            .ToListAsync(cancellationToken);

        // Read child2 list
        var child2List = await _dbContext.FamilyChild2
            .Where(c => c.ParentId == parent.Id)
            .ToListAsync(cancellationToken);

        Assert.HasCount(2, child1List);
        Assert.HasCount(3, child2List);

        foreach (var c in child1List)
            _logger.LogInformation("{Message}", $"FamilyChild1 Id={c.Id}, Name={c.Name}, ParentId={c.ParentId}");
        foreach (var c in child2List)
            _logger.LogInformation("{Message}", $"FamilyChild2 Id={c.Id}, Name={c.Name}, ParentId={c.ParentId}");
    }

    [TestMethod]
    public async Task Read_ParentWithChildren()
    {
        var cancellationToken = default(CancellationToken);

        // Seed
        var parent = new FamilyParent
        {
            Name = "Parent-WithChildren",
            FamilyChild1 =
            [
                new FamilyChild1 { Name = "Child1-W1" },
            ],
            // FamilyChild2 =
            // [
            //     new FamilyChild2 { Name = "Child2-W1" },
            // ],
        };
        await _dbContext.FamilyParent.AddAsync(parent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Read parent with both child collections via Include
        var found = await _dbContext.FamilyParent
            .Where(p => p.Id == parent.Id)
            .Include(p => p.FamilyChild1)
            // .Include(p => p.FamilyChild2)
            .FirstOrDefaultAsync(cancellationToken);

        Assert.IsNotNull(found);
        Assert.HasCount(1, found.FamilyChild1);
        // Assert.HasCount(1, found.FamilyChild2);

        _logger.LogInformation("{Message}", $"FamilyParent Id={found.Id}, Name={found.Name}");
        foreach (var c in found.FamilyChild1)
            _logger.LogInformation("{Message}", $"  FamilyChild1 Id={c.Id}, Name={c.Name}");
        // foreach (var c in found.FamilyChild2)
        //     Console.WriteLine($"  FamilyChild2 Id={c.Id}, Name={c.Name}");
    }
}
