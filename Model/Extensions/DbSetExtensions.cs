using Model.IEntities;

namespace Model.Extensions;

public static class DbSetExtensions
{
    public static IQueryable<T> EmptySqlRaw<T>(this DbSet<T> dbSet) where T : class
        => dbSet.FromSqlRaw("SELECT 1");

    public static IQueryable<T> WhereParentId<T>(this DbSet<T> dbSet, Guid parentId)
        where T : class, IFamilyChildEntite
        => dbSet.Where(e => e.ParentId == parentId);

    public static IQueryable<T> WhereParentIds<T>(this DbSet<T> dbSet, IEnumerable<Guid> parentIds)
        where T : class, IFamilyChildEntite
        => dbSet.Where(e => parentIds.Contains(e.ParentId));

    public static IQueryable<T> OrderByParentId<T>(this IQueryable<T> query)
        where T : IFamilyChildEntite
        => query.OrderBy(e => e.ParentId);

    public static IQueryable<T> OrderByParentIdDescending<T>(this IQueryable<T> query)
        where T : IFamilyChildEntite
        => query.OrderByDescending(e => e.ParentId);
}

