using Model.Definitions;
using Model.JsonObjects;

namespace Model.Entities;

public class VehicleLargeCar : VehicleBase
{
    public string? CarName { get; set; }
    public string? LargeCarName { get; set; }
    public LargeCarContentJsonObject Content { get; set; } = null!;
    public VehicleLargeCarType CarType
    {
        get => (VehicleLargeCarType)VehicleCarType;
        set => VehicleCarType = (int)value;
    }
}
