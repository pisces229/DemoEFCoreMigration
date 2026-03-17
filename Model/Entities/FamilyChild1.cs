using Model.IEntities;

namespace Model.Entities;

public class FamilyChild1 : ICreateEntite, IUpdateEntite, IFamilyChildEntite
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long ParentId { get; set; }

    public FamilyParent FamilyParent { get; set; } = null!;
}
