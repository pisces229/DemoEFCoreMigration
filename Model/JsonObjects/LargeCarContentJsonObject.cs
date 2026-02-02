using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Definitions;
using System.Text.Json;


namespace Model.JsonObjects;


public record LargeCarContentJsonObject(VehicleLargeCarType Type, string LargeCarContent);

public class LargeCarContentJsonObjectConverter : ValueConverter<LargeCarContentJsonObject?, string>
{
    public LargeCarContentJsonObjectConverter() : base(
        v => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions),
        v => JsonSerializer.Deserialize<LargeCarContentJsonObject>(v, DbContextUtil.JsonOptions)!
    )
    { }
}

public class LargeCarContentJsonObjectListConverter : ValueConverter<IEnumerable<LargeCarContentJsonObject>, string>
{
    public LargeCarContentJsonObjectListConverter() : base(
        v => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions),
        v => JsonSerializer.Deserialize<List<LargeCarContentJsonObject>>(v, DbContextUtil.JsonOptions) ?? new List<LargeCarContentJsonObject>()
    )
    { }
}

public class LargeCarContentJsonObjectComparer : ValueComparer<LargeCarContentJsonObject?>
{
    public LargeCarContentJsonObjectComparer() : base(
        (l, r) => object.Equals(l, r),
        v => v == null ? 0 : v.GetHashCode(),
        v => v
    )
    { }
}

public class LargeCarContentJsonObjectListComparer : ValueComparer<IEnumerable<LargeCarContentJsonObject>>
{
    public LargeCarContentJsonObjectListComparer() : base(
        (l, r) => l != null && r != null ? l.SequenceEqual(r) : l == r,
        v => v.Aggregate(0, (a, b) => HashCode.Combine(a, b.GetHashCode())),
        v => v.ToList()
    )
    { }
}
