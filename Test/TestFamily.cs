using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Test;

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

    [TestMethod(DisplayName = "Family_Write_Parent")]
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
        Console.WriteLine($"FamilyParent Id={parent.Id}, Name={parent.Name}");
    }

    [TestMethod(DisplayName = "Family_Write_Children")]
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

        Console.WriteLine($"FamilyChild1 Id={child1.Id}, Name={child1.Name}, ParentId={child1.ParentId}");
        Console.WriteLine($"FamilyChild2 Id={child2.Id}, Name={child2.Name}, ParentId={child2.ParentId}");
    }

    [TestMethod(DisplayName = "Family_Write_WithNavigation")]
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
            Console.WriteLine($"FamilyChild1 Id={c.Id}, Name={c.Name}, ParentId={c.ParentId}");
        // foreach (var c in parent.FamilyChild2)
        //     Console.WriteLine($"FamilyChild2 Id={c.Id}, Name={c.Name}, ParentId={c.ParentId}");
    }

    [TestMethod(DisplayName = "Family_Read_Parent")]
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
        Console.WriteLine($"Read FamilyParent Id={found.Id}, Name={found.Name}");
    }

    [TestMethod(DisplayName = "Family_Read_ChildrenByParent")]
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
            FamilyChild2 =
            [
                new FamilyChild2 { Name = "Child2-R1" },
                new FamilyChild2 { Name = "Child2-R2" },
                new FamilyChild2 { Name = "Child2-R3" },
            ],
        };
        await _dbContext.FamilyParent.AddAsync(parent, cancellationToken);
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
            Console.WriteLine($"FamilyChild1 Id={c.Id}, Name={c.Name}, ParentId={c.ParentId}");
        foreach (var c in child2List)
            Console.WriteLine($"FamilyChild2 Id={c.Id}, Name={c.Name}, ParentId={c.ParentId}");
    }

    [TestMethod(DisplayName = "Family_Read_ParentWithChildren")]
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

        Console.WriteLine($"FamilyParent Id={found.Id}, Name={found.Name}");
        foreach (var c in found.FamilyChild1)
            Console.WriteLine($"  FamilyChild1 Id={c.Id}, Name={c.Name}");
        // foreach (var c in found.FamilyChild2)
        //     Console.WriteLine($"  FamilyChild2 Id={c.Id}, Name={c.Name}");
    }
}
