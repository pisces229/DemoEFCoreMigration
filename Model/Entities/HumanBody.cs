using Model.Definitions;

namespace Model.Entities;

public partial class HumanBody
{
    public long Id { get; set; }
    public string? Ulid { get; set; }
    public int Weight { get; set; }
    public Color Color { get; set; }
    public DateTime CheckDate { get; set; }
    public string? Remark { get; set; }
    public string? HeadId { get; set; }
    public virtual HumanHead? HumanHead { get; set; }
    public virtual ICollection<HumanLimb>? HumanLimbs { get; set; }
}
