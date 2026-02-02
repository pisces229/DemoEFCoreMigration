using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Model.JsonObjects;

public record ValueJsonObject(DateTime StartDate, DateTime EndDate);

public class ValueJsonObjectConverter : ValueConverter<ValueJsonObject?, string>
{
    public ValueJsonObjectConverter() : base(
        v => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions),
        v => JsonSerializer.Deserialize<ValueJsonObject>(v, DbContextUtil.JsonOptions)!
    )
    { }
}

public class ValueJsonObjectListConverter : ValueConverter<IEnumerable<ValueJsonObject>, string>
{
    public ValueJsonObjectListConverter() : base(
        v => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions),
        v => JsonSerializer.Deserialize<List<ValueJsonObject>>(v, DbContextUtil.JsonOptions) ?? new List<ValueJsonObject>()
    )
    { }
}

public class ValueJsonObjectComparer : ValueComparer<ValueJsonObject?>
{
    public ValueJsonObjectComparer() : base(
        (l, r) => object.Equals(l, r),
        v => v == null ? 0 : v.GetHashCode(),
        v => v
    )
    { }
}

public class ValueJsonObjectListComparer : ValueComparer<IEnumerable<ValueJsonObject>>
{
    public ValueJsonObjectListComparer() : base(
        (l, r) => l != null && r != null ? l.SequenceEqual(r) : l == r,
        v => v.Aggregate(0, (a, b) => HashCode.Combine(a, b.GetHashCode())),
        v => v.ToList()
    )
    { }
}
