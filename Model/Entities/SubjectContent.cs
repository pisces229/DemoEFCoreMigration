namespace Model.Entities;

public abstract class SubjectContent
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public SubjectContentReferenceType ReferenceType { get; set; }
    public Guid ReferenceId { get; set; }
    public string Content { get; set; } = null!;
}
