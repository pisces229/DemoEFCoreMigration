using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Definitions;
using System.Text.Json;


namespace Model.JsonObjects;


public record SmallCarContentJsonObject(VehicleSmallCarType Type, string SmallCarContent);

public class SmallCarContentJsonObjectConverter : ValueConverter<SmallCarContentJsonObject?, string>
{
    public SmallCarContentJsonObjectConverter() : base(
        v => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions),
        v => JsonSerializer.Deserialize<SmallCarContentJsonObject>(v, DbContextUtil.JsonOptions)!
    )
    { }
}

public class SmallCarContentJsonObjectListConverter : ValueConverter<IEnumerable<SmallCarContentJsonObject>, string>
{
    public SmallCarContentJsonObjectListConverter() : base(
        v => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions),
        v => JsonSerializer.Deserialize<List<SmallCarContentJsonObject>>(v, DbContextUtil.JsonOptions) ?? new List<SmallCarContentJsonObject>()
    )
    { }
}

public class SmallCarContentJsonObjectComparer : ValueComparer<SmallCarContentJsonObject?>
{
    public SmallCarContentJsonObjectComparer() : base(
        (l, r) => object.Equals(l, r),
        v => v == null ? 0 : v.GetHashCode(),
        v => v
    )
    { }
}

public class SmallCarContentJsonObjectListComparer : ValueComparer<IEnumerable<SmallCarContentJsonObject>>
{
    public SmallCarContentJsonObjectListComparer() : base(
        (l, r) => l != null && r != null ? l.SequenceEqual(r) : l == r,
        v => v.Aggregate(0, (a, b) => HashCode.Combine(a, b.GetHashCode())),
        v => v.ToList()
    )
    { }
}
