namespace Model.Entities;

public class LinkFirstContent
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = null!;
    public virtual ICollection<LinkFirstSubContent> LinkFirstSubContents { get; set; } = [];
}
