using Microsoft.Extensions.Logging;

namespace IntegrationTest;

/// <summary>
/// SubjectFirst / SubjectSecond 與 polymorphic <see cref="SubjectContent"/>(透過 ReferenceType
/// 區分)的寫入、Include 查詢、GroupJoin 與級聯刪除驗證。
/// FK 名稱以 <c>d_</c> 開頭,需經由 fixture 的 DropConstraint 步驟移除才能跨型別寫入。
/// </summary>
[TestClass]
public class TestSubject : BaseTest
{
    [TestMethod]
    public async Task Create()
    {
        await _dbContext.SubjectFirst.AddRangeAsync([
            new SubjectFirst
            {
                Name = "SubjectFirst-1",
                Contents =
                [
                    new SubjectFirstContent { Content = "Content-1" },
                    new SubjectFirstContent { Content = "Content-2" },
                ]
            },
            new SubjectFirst
            {
                Name = "SubjectFirst-2",
                Contents =
                [
                    new SubjectFirstContent { Content = "Content-3" },
                    new SubjectFirstContent { Content = "Content-4" },
                ]
            },
        ]);
        await _dbContext.SubjectSecond.AddRangeAsync([
            new SubjectSecond
            {
                Name = "SubjectSecond-1",
                Contents =
                [
                    new SubjectSecondContent { Content = "Content-5" },
                    new SubjectSecondContent { Content = "Content-6" },
                ]
            },
            new SubjectSecond
            {
                Name = "SubjectSecond-2",
                Contents =
                [
                    new SubjectSecondContent { Content = "Content-7" },
                    new SubjectSecondContent { Content = "Content-8" },
                ]
            },
        ]);
        await _dbContext.SaveChangesAsync();
    }

    [TestMethod]
    public async Task SubjectInclude()
    {
        {
            var entities = await _dbContext.SubjectFirst.Include(e => e.Contents).ToListAsync();
            foreach (var entity in entities)
            {
                _logger.LogInformation("{Message}", entity.Name);
                foreach (var content in entity.Contents)
                {
                    _logger.LogInformation("{Message}", content.Content);
                }
            }
        }
        {
            var entities = await _dbContext.SubjectSecond.Include(e => e.Contents).ToListAsync();
            foreach (var entity in entities)
            {
                _logger.LogInformation("{Message}", entity.Name);
                foreach (var content in entity.Contents)
                {
                    _logger.LogInformation("{Message}", content.Content);
                }
            }
        }
    }

    [TestMethod]
    public async Task Subject()
    {
        {
            var entities = await _dbContext.SubjectFirst
                .GroupJoin(
                    _dbContext.SubjectFirstContent,
                    s => s.Id,
                    c => c.ReferenceId,
                    (s, cGroup) => new
                    {
                        Subject = s,
                        Contents = cGroup
                    }
                )
                .ToListAsync();
            foreach (var entity in entities)
            {
                _logger.LogInformation("{Message}", entity.Subject.Name);
                foreach (var content in entity.Contents)
                {
                    _logger.LogInformation("{Message}", content.Content);
                }
            }
        }
        {
            var entities = await _dbContext.SubjectSecond
                .GroupJoin(
                    _dbContext.SubjectSecondContent,
                    s => s.Id,
                    c => c.ReferenceId,
                    (s, cGroup) => new
                    {
                        Subject = s,
                        Contents = cGroup
                    }
                )
                .ToListAsync();
            foreach (var entity in entities)
            {
                _logger.LogInformation("{Message}", entity.Subject.Name);
                foreach (var content in entity.Contents)
                {
                    _logger.LogInformation("{Message}", content.Content);
                }
            }
        }
    }

    [TestMethod]
    public async Task SubjectContent()
    {
        {
            var entities = await _dbContext.SubjectContent.ToListAsync();
            foreach (var entity in entities)
            {
                _logger.LogInformation("{Message}", entity.ReferenceId);
                _logger.LogInformation("{Message}", entity.ReferenceType);
                _logger.LogInformation("{Message}", entity.Content);
            }
        }
        {
            var entities = await _dbContext.SubjectFirstContent.ToListAsync();
            foreach (var entity in entities)
            {
                _logger.LogInformation("{Message}", entity.ReferenceId);
                _logger.LogInformation("{Message}", entity.ReferenceType);
                _logger.LogInformation("{Message}", entity.Content);
            }
        }
        {
            var entities = await _dbContext.SubjectSecondContent.ToListAsync();
            foreach (var entity in entities)
            {
                _logger.LogInformation("{Message}", entity.ReferenceId);
                _logger.LogInformation("{Message}", entity.ReferenceType);
                _logger.LogInformation("{Message}", entity.Content);
            }
        }
    }

    [TestMethod]
    public async Task RemoveFirstContent()
    {
        var entities = await _dbContext.SubjectFirst
            .Include(e => e.Contents)
            .ToListAsync();
        _dbContext.SubjectFirst.RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }

}
