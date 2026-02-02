using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Model;

#region DateTime

#endregion

#region IEnumerable<string>

public class StringCollectionComparer : ValueComparer<IEnumerable<string>?>
{
    public StringCollectionComparer() : base(
        (c1, c2) => c1 == null && c2 == null ? true : c1 != null && c2 != null && c1.SequenceEqual(c2),
        c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v == null ? 0 : v.GetHashCode())),
        c => c == null ? null : c.ToList()
    )
    { }
}

#endregion
