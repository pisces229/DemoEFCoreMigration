using System.Text.Json.Serialization;

namespace Model.JsonObjects;

public record ValueJsonObject(
    [property: JsonPropertyName("start_date")] DateTime StartDate,
    [property: JsonPropertyName("end_date")] DateTime EndDate
);

public class ValueJsonObjectConverter : JsonValueConverter<ValueJsonObject>
{
}

public class ValueJsonObjectListConverter : JsonValueListConverter<ValueJsonObject>
{
}

public class ValueJsonObjectComparer : JsonValueComparer<ValueJsonObject>
{
}

public class ValueJsonObjectListComparer : JsonValueListComparer<ValueJsonObject>
{
}
