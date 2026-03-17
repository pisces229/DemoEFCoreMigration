namespace Model.Entities;

public class SubjectFirst
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<SubjectFirstContent> Contents { get; set; } = [];
}
