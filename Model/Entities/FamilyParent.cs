using Model.IEntities;

namespace Model.Entities;

public class FamilyParent : ICreateEntite, IUpdateEntite
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<FamilyChild1> FamilyChild1 { get; set; } = [];
    // public virtual ICollection<FamilyChild2> FamilyChild2 { get; set; } = [];
}
