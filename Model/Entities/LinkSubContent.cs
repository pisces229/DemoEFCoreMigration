namespace Model.Entities;

public abstract class LinkSubContent
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public LinkSubContentLinkType LinkType { get; set; }
    public string Content { get; set; } = null!;
}
