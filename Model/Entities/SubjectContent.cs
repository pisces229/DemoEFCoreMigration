using Model.Definitions;

namespace Model.Entities;

public abstract class SubjectContent
{
    public long Id { get; set; }
    public SubjectContentReferenceType ReferenceType { get; set; }
    public long ReferenceId { get; set; }
    public string Content { get; set; } = null!;
}
