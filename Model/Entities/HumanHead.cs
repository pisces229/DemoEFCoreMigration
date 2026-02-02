using Model.Definitions;

namespace Model.Entities;

public partial class HumanHead
{
    public long Id { get; set; }
    public string? Ulid { get; set; }
    public int Weight { get; set; }
    public Color Color { get; set; }
    public DateTime CheckDate { get; set; }
    public string? Remark { get; set; }
    public virtual HumanBody HumanBody { get; set; } = null!;
}
