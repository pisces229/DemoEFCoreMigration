namespace Model.Entities;

public partial class AppIndex
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string C1 { get; set; } = null!;
    public string C2 { get; set; } = null!;
}
