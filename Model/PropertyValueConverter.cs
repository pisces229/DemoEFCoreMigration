using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Model;

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
