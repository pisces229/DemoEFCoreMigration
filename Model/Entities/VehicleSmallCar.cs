using Model.Definitions;
using Model.JsonObjects;

namespace Model.Entities;

public class VehicleSmallCar : VehicleBase
{
    public string? CarName { get; set; }
    public string? SmallCarName { get; set; }
    public SmallCarContentJsonObject Content { get; set; } = null!;
    public VehicleSmallCarType CarType
    {
        get => (VehicleSmallCarType)VehicleCarType;
        set => VehicleCarType = (int)value;
    }
}
