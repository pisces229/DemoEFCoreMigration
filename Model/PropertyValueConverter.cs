using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Model;

/// <summary>
/// 寫入 DB 前統一轉 UTC、讀取保持原值。
/// Why: 搭配 PostgreSQL <c>timestamptz</c>(timestamp with time zone)欄位 —
/// 由 DB driver 處理時區還原,應用端只需保證寫入時為 UTC,避免 Local/Unspecified 混入導致時區漂移。
/// </summary>
internal class DateTimeWithZoneConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeWithZoneConverter() : base(
        v => v.ToUniversalTime(),
        v => v
    )
    { }
}

/// <summary>
/// <see cref="DateTimeWithZoneConverter"/> 的 nullable 版。
/// </summary>
internal class NullableDateTimeWithZoneConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeWithZoneConverter() : base(
        v => v.HasValue ? v.Value.ToUniversalTime() : default,
        v => v
    )
    { }
}

/// <summary>
/// 寫入 DB 前剝除 <see cref="DateTimeKind"/>,讀取後標為 <see cref="DateTimeKind.Local"/>。
/// Why: 搭配 PostgreSQL <c>timestamp</c>(timestamp without time zone)欄位 —
/// DB 不存時區資訊,Kind 由應用端依商業語義決定;若直接寫 UTC 而欄位為 timestamp,Npgsql 會丟例外。
/// </summary>
internal class DateTimeWithoutZoneConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeWithoutZoneConverter() : base(
        v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified),
        v => DateTime.SpecifyKind(v, DateTimeKind.Local)
    )
    { }
}

/// <summary>
/// <see cref="DateTimeWithoutZoneConverter"/> 的 nullable 版。
/// </summary>
internal class NullableDateTimeWithoutZoneConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeWithoutZoneConverter() : base(
        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Unspecified) : v,
        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Local) : v
    )
    { }
}

/// <summary>
/// JSONB 欄位映射用 ValueConverter 共用基底。
/// Why: <c>Model/JsonObjects/*</c> 下成對的 *Converter / *ListConverter body 完全雷同,
/// 抽出後子類只需單行繼承宣告(鏡像 <see cref="JsonValueComparer{T}"/> 模式)。
/// </summary>
public abstract class JsonValueConverter<T> : ValueConverter<T?, string>
    where T : class
{
    protected JsonValueConverter()
        : base(
            v => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions),
            v => JsonSerializer.Deserialize<T>(v, DbContextUtil.JsonOptions)!
        )
    { }
}

/// <summary>
/// JSONB 欄位映射用 List ValueConverter 共用基底:與 <see cref="JsonValueConverter{T}"/> 同源。
/// </summary>
public abstract class JsonValueListConverter<T> : ValueConverter<List<T>, string>
{
    protected JsonValueListConverter()
        : base(
            v => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions),
            v => JsonSerializer.Deserialize<List<T>>(v, DbContextUtil.JsonOptions) ?? new List<T>()
        )
    { }
}
