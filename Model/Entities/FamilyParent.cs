using Model.IEntities;

namespace Model.Entities;

public class FamilyParent : ICreateEntite, IUpdateEntite
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<FamilyChild1> FamilyChild1 { get; set; } = [];
    // public virtual ICollection<FamilyChild2> FamilyChild2 { get; set; } = [];
}
