namespace Model.Entities;

public abstract class TablePerHierarchy
{
    public long Id { get; set; }
    public string TypeName { get; } = null!;
    public string Name { get; set; } = null!;
}
