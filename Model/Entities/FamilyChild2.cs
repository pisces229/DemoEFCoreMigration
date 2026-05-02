using Model.IEntities;

namespace Model.Entities;

public class FamilyChild2 : ICreateEntite, IUpdateEntite, IFamilyChildEntite
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid ParentId { get; set; }

    // public FamilyParent FamilyParent { get; set; } = null!;
}
