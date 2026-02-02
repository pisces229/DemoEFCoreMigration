using Model.Definitions;
using Model.Entities;

namespace Model.Names;

internal class SubjectContentConstraintName
{
    public static Dictionary<SubjectContentReferenceType, string> Discriminators => new()
    {
        [SubjectContentReferenceType.First] = DbContextUtil.DropConstraintName(
            nameof(SubjectFirstContent),
            nameof(SubjectContent.ReferenceId)),
        [SubjectContentReferenceType.Second] = DbContextUtil.DropConstraintName(
            nameof(SubjectSecondContent),
            nameof(SubjectContent.ReferenceId))
    };
}
