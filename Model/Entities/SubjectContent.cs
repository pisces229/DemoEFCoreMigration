using Model.Definitions;

namespace Model.Entities;

public abstract class SubjectContent
{
    public long Id { get; set; }
    public long ParentId { get; set; }
    public SubjectContentType Type { get; set; }
    public string Content { get; set; } = null!;
}
