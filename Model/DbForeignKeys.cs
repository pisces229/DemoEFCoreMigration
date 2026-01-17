using Model.Entities;

namespace Model;

internal static class SubjectFirstContentForeignKey
{
    public static readonly string ParentId = DbContextUtil.CreateForeignKey(nameof(SubjectFirstContent), nameof(SubjectFirstContent.ParentId));
}
internal static class SubjectSecondContentForeignKey
{
    public static readonly string ParentId = DbContextUtil.CreateForeignKey(nameof(SubjectSecondContent), nameof(SubjectSecondContent.ParentId));
}
