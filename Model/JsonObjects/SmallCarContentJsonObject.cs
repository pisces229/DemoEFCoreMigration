namespace Model.JsonObjects;

public record SmallCarContentJsonObject(VehicleSmallCarType Type, string SmallCarContent);

public class SmallCarContentJsonObjectConverter : JsonValueConverter<SmallCarContentJsonObject>
{
}

public class SmallCarContentJsonObjectListConverter : JsonValueListConverter<SmallCarContentJsonObject>
{
}

public class SmallCarContentJsonObjectComparer : JsonValueComparer<SmallCarContentJsonObject>
{
}

public class SmallCarContentJsonObjectListComparer : JsonValueListComparer<SmallCarContentJsonObject>
{
}
