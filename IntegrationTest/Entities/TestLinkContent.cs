namespace IntegrationTest;

/// <summary>
/// LinkFirstContent / LinkSecondContent 與其 SubContent(LinkFirstSubContent / LinkSecondSubContent)
/// 的多型 sub-content 寫入與級聯刪除測試。
/// </summary>
[TestClass]
public class TestLinkContent : BaseTest
{
    [TestMethod]
    public async Task CreateContent()
    {
        await _dbContext.LinkFirstContent.AddRangeAsync(new LinkFirstContent()
        {
            Name = "First Content 1",
            LinkFirstSubContents =
            [
                new()
                {
                    Content = "First Sub Content 1"
                },
                new()
                {
                    Content = "First Sub Content 2"
                }
            ]
        });
        await _dbContext.LinkSecondContent.AddRangeAsync(new LinkSecondContent()
        {
            Name = "Second Content 1",
            LinkSecondSubContents =
            [
                new()
                {
                    Content = "Second Sub Content 1"
                },
                new()
                {
                    Content = "Second Sub Content 2"
                }
            ]
        });
        await _dbContext.SaveChangesAsync();
    }

    [TestMethod]
    public async Task SubContentCreate()
    {
        {
            var contents = await _dbContext.LinkFirstContent.Include(e => e.LinkFirstSubContents).ToListAsync();
            foreach (var content in contents)
            {
                content.LinkFirstSubContents.Add(new LinkFirstSubContent()
                {
                    Content = "First Sub Content New"
                });
            }
        }
        {
            var contents = await _dbContext.LinkSecondContent.Include(e => e.LinkSecondSubContents).ToListAsync();
            foreach (var content in contents)
            {
                content.LinkSecondSubContents.Add(new LinkSecondSubContent()
                {
                    Content = "Second Sub Content New"
                });
            }
        }
        await _dbContext.SaveChangesAsync();
    }

    [TestMethod]
    public async Task SubContentRemove()
    {
        {
            var contents = await _dbContext.LinkFirstContent.Include(e => e.LinkFirstSubContents).ToListAsync();
            foreach (var content in contents)
            {
                if (content.LinkFirstSubContents.Any())
                {
                    content.LinkFirstSubContents.Remove(content.LinkFirstSubContents.First());
                }
            }
        }
        {
            var contents = await _dbContext.LinkSecondContent.Include(e => e.LinkSecondSubContents).ToListAsync();
            foreach (var content in contents)
            {
                if (content.LinkSecondSubContents.Any())
                {
                    content.LinkSecondSubContents.Remove(content.LinkSecondSubContents.First());
                }
            }
        }
        await _dbContext.SaveChangesAsync();
    }

    [TestMethod]
    public async Task Get()
    {
        await _dbContext.LinkFirstContent.Include(e => e.LinkFirstSubContents).ToListAsync();
        await _dbContext.LinkSecondContent.Include(e => e.LinkSecondSubContents).ToListAsync();
    }
}
