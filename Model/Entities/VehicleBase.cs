namespace Model.Entities;

public abstract class VehicleBase
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public VehicleType Type { get; set; }
    public string Name { get; set; } = null!;
    public int VehicleCarType { get; set; }
}
