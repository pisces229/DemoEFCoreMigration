namespace Model.Entities;

public partial class ClosurePath
{
    public long AncestorId { get; set; }
    public long DescendantId { get; set; }
    public int Depth { get; set; }

    public virtual ClosureNode Ancestor { get; set; } = null!;
    public virtual ClosureNode Descendant { get; set; } = null!;
}
