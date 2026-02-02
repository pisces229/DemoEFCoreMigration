using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Model;

#region DateTime

public class DateTimeWithZoneConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeWithZoneConverter() : base(
        v => v.ToUniversalTime(),
        v => v
    )
    { }
}

public class NullableDateTimeWithZoneConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeWithZoneConverter() : base(
        v => v.HasValue ? v.Value.ToUniversalTime() : default,
        v => v.HasValue ? v : default
    )
    { }
}

public class DateTimeWithoutZoneConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeWithoutZoneConverter() : base(
        v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified),
        v => DateTime.SpecifyKind(v, DateTimeKind.Local)
    )
    { }
}

public class NullableDateTimeWithoutZoneConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeWithoutZoneConverter() : base(
        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Unspecified) : v,
        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Local) : v
    )
    { }
}

#endregion

#region IEnumerable<string>

public class StringCollectionConverter : ValueConverter<IEnumerable<string>?, string?>
{
    public StringCollectionConverter() : base(
        v => v == null ? null : JsonSerializer.Serialize(v, DbContextUtil.JsonOptions),
        v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<string>>(v, DbContextUtil.JsonOptions)
    )
    { }
}

#endregion
