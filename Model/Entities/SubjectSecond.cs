namespace Model.Entities;

public class SubjectSecond
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = null!;
    public virtual ICollection<SubjectSecondContent> Contents { get; set; } = [];
}
