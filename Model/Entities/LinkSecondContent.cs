namespace Model.Entities;

public class LinkSecondContent
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<LinkSecondSubContent> LinkSecondSubContents { get; set; } = [];
}
