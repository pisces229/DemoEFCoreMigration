namespace Model.Definitions;

public enum DataType
{
    First,
    Second,
    Third,
}
public enum Color
{
    Red,
    Yellow,
    Green,
}

[Flags]
public enum Flag
{
    None = 0,
    First = 1 << 0,
    Second = 1 << 1,
    Third = 1 << 2,
    Fourth = 1 << 3,
}

public enum VehicleType
{
    None,
    SmallCar,
    LargeCar,
}

public enum VehicleSmallCarType
{
    None = 10,
    Sedan = 11,
    CompactCar = 12,
}

public enum VehicleLargeCarType
{
    None = 20,
    Bus = 21,
    Coach = 22,
}
