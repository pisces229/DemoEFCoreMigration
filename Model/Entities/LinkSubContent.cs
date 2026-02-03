using Model.Definitions;

namespace Model.Entities;

public abstract class LinkSubContent
{
    public long Id { get; set; }
    public LinkSubContentLinkType LinkType { get; set; }
    public string Content { get; set; } = null!;
}
