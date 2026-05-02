namespace Model.JsonObjects;

public record LargeCarContentJsonObject(VehicleLargeCarType Type, string LargeCarContent);

public class LargeCarContentJsonObjectConverter : JsonValueConverter<LargeCarContentJsonObject>
{
}

public class LargeCarContentJsonObjectListConverter : JsonValueListConverter<LargeCarContentJsonObject>
{
}

public class LargeCarContentJsonObjectComparer : JsonValueComparer<LargeCarContentJsonObject>
{
}

public class LargeCarContentJsonObjectListComparer : JsonValueListComparer<LargeCarContentJsonObject>
{
}
