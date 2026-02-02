using Microsoft.EntityFrameworkCore;

namespace Model;

public static class DbSetExtensions
{
    public static IQueryable<T> EmptySqlRaw<T>(this DbSet<T> dbSet) where T : class
        => dbSet.FromSqlRaw("SELECT 1");
}
