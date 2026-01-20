using Model.Definitions;
using Model.Entities;

namespace Model.Names;

internal class SubjectContentName
{
    protected static string TableName => nameof(SubjectContent);
}

internal class SubjectContentDatabaseName : SubjectContentName
{
    // add index names here if needed
}

internal class SubjectContentConstraintName : SubjectContentName
{
    //public static string ReferenceId => DbContextUtil.CreateForeignKey(TableName, nameof(SubjectContent.ReferenceId));

    public static Dictionary<SubjectContentReferenceType, string> Discriminators => new()
    {
        [SubjectContentReferenceType.First] = DbContextUtil.CreateForeignKey(
            nameof(SubjectFirstContent),
            nameof(SubjectContent.ReferenceId)),
        [SubjectContentReferenceType.Second] = DbContextUtil.CreateForeignKey(
            nameof(SubjectSecondContent),
            nameof(SubjectContent.ReferenceId))
    };

    public static string DropDiscriminators =>
        DbContextUtil.DropConstraintScript(TableName, Discriminators.Values);
}
