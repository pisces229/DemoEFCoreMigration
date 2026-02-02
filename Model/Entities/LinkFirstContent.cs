namespace Model.Entities;

public class LinkFirstContent
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<LinkFirstSubContent> LinkFirstSubContents { get; set; } = [];
}
