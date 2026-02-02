using Model.Definitions;

namespace Model.Entities;

public abstract class VehicleBase
{
    public long Id { get; set; }
    public VehicleType Type { get; }
    public string Name { get; set; } = null!;
    public int VehicleCarType { get; set; }
}
