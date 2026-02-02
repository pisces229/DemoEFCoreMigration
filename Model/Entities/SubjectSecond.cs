namespace Model.Entities;

public class SubjectSecond
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<SubjectSecondContent> Contents { get; set; } = [];
}
