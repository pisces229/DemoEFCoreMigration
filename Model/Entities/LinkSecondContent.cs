namespace Model.Entities;

public class LinkSecondContent
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = null!;
    public virtual ICollection<LinkSecondSubContent> LinkSecondSubContents { get; set; } = [];
}
