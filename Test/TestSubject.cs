using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Test;

[TestClass]
public class TestSubject : BaseTest
{
    [TestMethod(DisplayName = "Create")]
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

    [TestMethod(DisplayName = "SubjectInclude")]
    public async Task SubjectInclude()
    {
        {
            var entities = await _dbContext.SubjectFirst.Include(e => e.Contents).ToListAsync();
            foreach (var entity in entities)
            {
                Console.WriteLine(entity.Name);
                foreach (var content in entity.Contents)
                {
                    Console.WriteLine(content.Content);
                }
            }
        }
        {
            var entities = await _dbContext.SubjectSecond.Include(e => e.Contents).ToListAsync();
            foreach (var entity in entities)
            {
                Console.WriteLine(entity.Name);
                foreach (var content in entity.Contents)
                {
                    Console.WriteLine(content.Content);
                }
            }
        }
    }

    [TestMethod(DisplayName = "Subject")]
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
                Console.WriteLine(entity.Subject.Name);
                foreach (var content in entity.Contents)
                {
                    Console.WriteLine(content.Content);
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
                Console.WriteLine(entity.Subject.Name);
                foreach (var content in entity.Contents)
                {
                    Console.WriteLine(content.Content);
                }
            }
        }
    }

    [TestMethod(DisplayName = "SubjectContent")]
    public async Task SubjectContent()
    {
        {
            var entities = await _dbContext.SubjectContent.ToListAsync();
            foreach (var entity in entities)
            {
                Console.WriteLine(entity.ReferenceId);
                Console.WriteLine(entity.ReferenceType);
                Console.WriteLine(entity.Content);
            }
        }
        {
            var entities = await _dbContext.SubjectFirstContent.ToListAsync();
            foreach (var entity in entities)
            {
                Console.WriteLine(entity.ReferenceId);
                Console.WriteLine(entity.ReferenceType);
                Console.WriteLine(entity.Content);
            }
        }
        {
            var entities = await _dbContext.SubjectSecondContent.ToListAsync();
            foreach (var entity in entities)
            {
                Console.WriteLine(entity.ReferenceId);
                Console.WriteLine(entity.ReferenceType);
                Console.WriteLine(entity.Content);
            }
        }
    }

    [TestMethod(DisplayName = "RemoveFirstContent")]
    public async Task RemoveFirstContent()
    {
        var entities = await _dbContext.SubjectFirst
            .Include(e => e.Contents)
            .ToListAsync();
        //_dbContext.SubjectFirstContent.RemoveRange(entities.SelectMany(s => s.Contents));
        //await _dbContext.SaveChangesAsync();
        _dbContext.SubjectFirst.RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }

}
