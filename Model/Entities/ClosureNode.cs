namespace Model.Entities;

public partial class ClosureNode
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = null!;

    public virtual ICollection<ClosurePath> AncestorPaths { get; set; } = [];
    public virtual ICollection<ClosurePath> DescendantPaths { get; set; } = [];
}
