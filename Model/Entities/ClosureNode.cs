namespace Model.Entities;

public partial class ClosureNode
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<ClosurePath> AncestorPaths { get; set; } = [];
    public virtual ICollection<ClosurePath> DescendantPaths { get; set; } = [];
}
