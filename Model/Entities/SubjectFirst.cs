namespace Model.Entities;

public class SubjectFirst
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = null!;
    public virtual ICollection<SubjectFirstContent> Contents { get; set; } = [];
}
