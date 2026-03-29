namespace Model.Entities;

public partial class ClosurePath
{
    public Guid AncestorId { get; set; }
    public Guid DescendantId { get; set; }
    public int Depth { get; set; }

    public virtual ClosureNode Ancestor { get; set; } = null!;
    public virtual ClosureNode Descendant { get; set; } = null!;
}
