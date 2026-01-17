using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model.ValueObjects;

namespace Model;

public class DateValueObjectComparer : ValueComparer<DateValueObject?>
{
    public DateValueObjectComparer() : base(
        (l, r) => object.Equals(l, r),
        v => v == null ? 0 : v.GetHashCode(),
        v => v
    )
    { }
}

public class DateValueObjectListComparer : ValueComparer<IEnumerable<DateValueObject>>
{
    public DateValueObjectListComparer() : base(
        (l, r) => l != null && r != null ? l.SequenceEqual(r) : l == r,
        v => v.Aggregate(0, (a, b) => HashCode.Combine(a, b.GetHashCode())),
        v => v.ToList()
    )
    { }
}
