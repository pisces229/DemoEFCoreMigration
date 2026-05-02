using Microsoft.Extensions.Logging;

namespace IntegrationTest;

/// <summary>
/// Closure Table 樹結構(<see cref="ClosureNode"/>/<see cref="ClosurePath"/>)的階層查詢、
/// 子樹搬移與深層節點重建路徑驗證。
/// </summary>
[TestClass]
public class TestClosureTable : BaseTest
{
    private async Task SeedHierarchy(ClosureNode parentNode, int currentDepth, int maxDepth, int childrenPerNode)
    {
        if (currentDepth >= maxDepth) return;

        for (int i = 0; i < childrenPerNode; i++)
        {
            var childNode = new ClosureNode { Name = $"{parentNode.Name}_{i + 1}" };
            _dbContext.ClosureNode.Add(childNode);
            await _dbContext.SaveChangesAsync();

            // Self path
            _dbContext.ClosurePath.Add(new ClosurePath
            {
                AncestorId = childNode.Id,
                DescendantId = childNode.Id,
                Depth = 0
            });

            // Paths from ancestors
            var ancestorPaths = await _dbContext.ClosurePath
                .Where(p => p.DescendantId == parentNode.Id)
                .ToListAsync();

            foreach (var ancestorPath in ancestorPaths)
            {
                _dbContext.ClosurePath.Add(new ClosurePath
                {
                    AncestorId = ancestorPath.AncestorId,
                    DescendantId = childNode.Id,
                    Depth = ancestorPath.Depth + 1
                });
            }
            await _dbContext.SaveChangesAsync();

            // Recurse
            await SeedHierarchy(childNode, currentDepth + 1, maxDepth, childrenPerNode);
        }
    }

    private async Task<ClosureNode> SeedRootAndHierarchy(int maxDepth, int childrenPerNode)
    {
        var rootNode = new ClosureNode { Name = "Root" };
        _dbContext.ClosureNode.Add(rootNode);
        await _dbContext.SaveChangesAsync();

        _dbContext.ClosurePath.Add(new ClosurePath
        {
            AncestorId = rootNode.Id,
            DescendantId = rootNode.Id,
            Depth = 0
        });
        await _dbContext.SaveChangesAsync();

        await SeedHierarchy(rootNode, 1, maxDepth, childrenPerNode);
        return rootNode;
    }

    public override async Task TestCleanup()
    {
        var cancellationToken = default(CancellationToken);
        await _dbContext.ClosurePath.ExecuteDeleteAsync(cancellationToken);
        await _dbContext.ClosureNode.ExecuteDeleteAsync(cancellationToken);
        await base.TestCleanup();
    }

    [TestMethod]
    public async Task ClosureTable_AddAndRemove()
    {
        var rootNode = await SeedRootAndHierarchy(maxDepth: 3, childrenPerNode: 3);

        // Print paths
        var paths = await _dbContext.ClosurePath
            .Include(p => p.Ancestor)
            .Include(p => p.Descendant)
            .OrderBy(p => p.AncestorId).ThenBy(p => p.DescendantId)
            .ToListAsync();

        _logger.LogInformation("{Message}", $"Total paths created: {paths.Count}");
        // Commenting this out because 3 branches 3 deep is too large to print nicely!
        foreach (var path in paths.Take(20))
        {
            _logger.LogInformation("{Message}", $"{path.Ancestor.Name} -> {path.Descendant.Name} (Depth: {path.Depth})");
        }

        // Verify initial state
        Assert.IsNotNull(rootNode);
        Assert.IsTrue(paths.Count > 0, "Should have created closure paths");
    }

    [TestMethod]
    public async Task ClosureTable_Move()
    {
        var rootNode = await SeedRootAndHierarchy(maxDepth: 3, childrenPerNode: 3);

        // Fetch nodes to manipulate: e.g. Root_1 and its child Root_1_1 (which is child 1 of Root_1)
        var sourceParentNode = await _dbContext.ClosureNode.FirstAsync(n => n.Name == "Root_1");
        var movingNode = await _dbContext.ClosureNode.FirstAsync(n => n.Name == "Root_1_1");
        var destinationParentNode = await _dbContext.ClosureNode.FirstAsync(n => n.Name == "Root_2");

        // Move movingNode to be a direct child of destinationParentNode
        // Disconnect movingNode and its descendants from movingNode's ancestors (excluding movingNode itself)
        var subtreeDescendantIds = _dbContext.ClosurePath
            .Where(p => p.AncestorId == movingNode.Id)
            .Select(p => p.DescendantId);

        var oldAncestorIds = _dbContext.ClosurePath
            .Where(p => p.DescendantId == movingNode.Id && p.AncestorId != movingNode.Id)
            .Select(p => p.AncestorId);

        // 使用 EF Core 7/8/9 的 ExecuteDelete 一次刪除路徑
        var deletedCount = await _dbContext.ClosurePath
            .Where(p => subtreeDescendantIds.Contains(p.DescendantId) && oldAncestorIds.Contains(p.AncestorId))
            .ExecuteDeleteAsync();

        // 清除追蹤狀態,避免 ExecuteDelete 後同一 Key 被追蹤而引發問題
        _dbContext.ChangeTracker.Clear();

        _logger.LogInformation("{Message}", $"Deleted {deletedCount} paths during disconnection.");

        // Insert new paths to connect destinationParentNode and its ancestors to movingNode and its descendants
        var superPaths = await _dbContext.ClosurePath
            .Where(p => p.DescendantId == destinationParentNode.Id)
            .ToListAsync();

        var subPaths = await _dbContext.ClosurePath
            .Where(p => p.AncestorId == movingNode.Id)
            .ToListAsync();

        var newPaths = new List<ClosurePath>();
        foreach (var super in superPaths)
        {
            foreach (var sub in subPaths)
            {
                newPaths.Add(new ClosurePath
                {
                    AncestorId = super.AncestorId,
                    DescendantId = sub.DescendantId,
                    Depth = super.Depth + sub.Depth + 1
                });
            }
        }

        _dbContext.ClosurePath.AddRange(newPaths);
        await _dbContext.SaveChangesAsync();

        // Print paths
        var paths = await _dbContext.ClosurePath
            .Include(p => p.Ancestor)
            .Include(p => p.Descendant)
            .OrderBy(p => p.AncestorId).ThenBy(p => p.DescendantId)
            .ToListAsync();

        _logger.LogInformation("{Message}", $"\nPaths after moving {movingNode.Name} under {destinationParentNode.Name}:");
        // Commenting this out for same reason
        foreach (var path in paths)
        {
            _logger.LogInformation("{Message}", $"{path.Ancestor.Name} -> {path.Descendant.Name} (Depth: {path.Depth})");
        }

        // Verify sourceParentNode is no longer ancestor of movingNode
        var sourceParentIsStillAncestor = await _dbContext.ClosurePath
            .AnyAsync(p => p.AncestorId == sourceParentNode.Id && p.DescendantId == movingNode.Id);
        Assert.IsFalse(sourceParentIsStillAncestor, $"{sourceParentNode.Name} should not be ancestor of {movingNode.Name} after move.");

        var destinationParentIsNowAncestor = await _dbContext.ClosurePath
            .AnyAsync(p => p.AncestorId == destinationParentNode.Id && p.DescendantId == movingNode.Id);
        Assert.IsTrue(destinationParentIsNowAncestor, $"{destinationParentNode.Name} should be ancestor of {movingNode.Name} after move.");

        // 清除資料
        await _dbContext.ClosurePath.ExecuteDeleteAsync();
        await _dbContext.ClosureNode.ExecuteDeleteAsync();
    }

    [TestMethod]
    public async Task ClosureTable_MoveToNewDeepNode()
    {
        // 建立深度 4 的樹結構 (Root + 3 層子節點)
        var rootNode = await SeedRootAndHierarchy(maxDepth: 4, childrenPerNode: 3);

        // 1. 挑選一個深度節點 (例如 Root_2_2_2,即原本深度 4 的節點)
        var deepParent = await _dbContext.ClosureNode.FirstAsync(n => n.Name == "Root_2_2_2");

        var newDeepNode = new ClosureNode { Name = "New_Deep_Node" };
        _dbContext.ClosureNode.Add(newDeepNode);
        await _dbContext.SaveChangesAsync();

        // 建立新節點的 Self Path
        _dbContext.ClosurePath.Add(new ClosurePath
        {
            AncestorId = newDeepNode.Id,
            DescendantId = newDeepNode.Id,
            Depth = 0
        });

        var deepParentAncestors = await _dbContext.ClosurePath
            .Where(p => p.DescendantId == deepParent.Id)
            .ToListAsync();

        foreach (var ancestorPath in deepParentAncestors)
        {
            _dbContext.ClosurePath.Add(new ClosurePath
            {
                AncestorId = ancestorPath.AncestorId,
                DescendantId = newDeepNode.Id,
                Depth = ancestorPath.Depth + 1
            });
        }
        await _dbContext.SaveChangesAsync();

        // 2. 將子樹(節點與其子孫節點)搬移,例如將 Root_1_1 移到 New_Deep_Node 下
        var movingNode = await _dbContext.ClosureNode.FirstAsync(n => n.Name == "Root_1_1");
        var sourceParentNode = await _dbContext.ClosureNode.FirstAsync(n => n.Name == "Root_1");

        _logger.LogInformation("{Message}", $"Moving {movingNode.Name} (and its descendants) to be under {newDeepNode.Name}");

        // Disconnect movingNode and its descendants from movingNode's old ancestors (excluding movingNode itself)
        var subtreeDescendantIds = _dbContext.ClosurePath
            .Where(p => p.AncestorId == movingNode.Id)
            .Select(p => p.DescendantId);

        var oldAncestorIds = _dbContext.ClosurePath
            .Where(p => p.DescendantId == movingNode.Id && p.AncestorId != movingNode.Id)
            .Select(p => p.AncestorId);

        // 使用 EF Core 7/8/9 的 ExecuteDelete 一次刪除路徑
        var deletedCount = await _dbContext.ClosurePath
            .Where(p => subtreeDescendantIds.Contains(p.DescendantId) && oldAncestorIds.Contains(p.AncestorId))
            .ExecuteDeleteAsync();

        // 清除追蹤狀態,避免 ExecuteDelete 後同一 Key 被追蹤而引發問題
        _dbContext.ChangeTracker.Clear();

        _logger.LogInformation("{Message}", $"Deleted {deletedCount} paths during disconnection.");

        // Insert new paths to connect newDeepNode and its ancestors to movingNode and its descendants
        var superPaths = await _dbContext.ClosurePath
            .Where(p => p.DescendantId == newDeepNode.Id)
            .ToListAsync();

        var subPaths = await _dbContext.ClosurePath
            .Where(p => p.AncestorId == movingNode.Id)
            .ToListAsync();

        var newPaths = new List<ClosurePath>();
        foreach (var super in superPaths)
        {
            foreach (var sub in subPaths)
            {
                newPaths.Add(new ClosurePath
                {
                    AncestorId = super.AncestorId,
                    DescendantId = sub.DescendantId,
                    Depth = super.Depth + sub.Depth + 1
                });
            }
        }

        _dbContext.ClosurePath.AddRange(newPaths);
        await _dbContext.SaveChangesAsync();

        // Print paths
        var paths = await _dbContext.ClosurePath
            .Include(p => p.Ancestor)
            .Include(p => p.Descendant)
            .OrderBy(p => p.AncestorId).ThenBy(p => p.DescendantId)
            .ToListAsync();
        // Commenting this out for same reason
        foreach (var path in paths)
        {
            _logger.LogInformation("{Message}", $"{path.Ancestor.Name} -> {path.Descendant.Name} (Depth: {path.Depth})");
        }

        // Verify the move was successful
        var newDeepNodeIsAncestor = await _dbContext.ClosurePath
            .AnyAsync(p => p.AncestorId == newDeepNode.Id && p.DescendantId == movingNode.Id);
        Assert.IsTrue(newDeepNodeIsAncestor, $"{newDeepNode.Name} should be ancestor of {movingNode.Name} after move.");

        var rootIsStillAncestor = await _dbContext.ClosurePath
            .AnyAsync(p => p.AncestorId == rootNode.Id && p.DescendantId == movingNode.Id);
        Assert.IsTrue(rootIsStillAncestor, $"Root should still be ancestor of {movingNode.Name}.");

        var sourceParentIsStillAncestor = await _dbContext.ClosurePath
            .AnyAsync(p => p.AncestorId == sourceParentNode.Id && p.DescendantId == movingNode.Id);
        Assert.IsFalse(sourceParentIsStillAncestor, $"{sourceParentNode.Name} should not be ancestor of {movingNode.Name} after move.");

        // 清除資料
        await _dbContext.ClosurePath.ExecuteDeleteAsync();
        await _dbContext.ClosureNode.ExecuteDeleteAsync();
    }
}