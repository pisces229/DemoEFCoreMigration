namespace Model.Entities;

public partial class HumanLimb
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Ulid { get; set; } = string.Empty;
    public int Weight { get; set; }
    public Color Color { get; set; }
    public DateTime CheckDate { get; set; }
    public string? Remark { get; set; }
    public Guid? BodyId { get; set; }
    public virtual HumanBody? HumanBody { get; set; }
}
