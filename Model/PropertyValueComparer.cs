using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Model;

/// <summary>
/// <c>List&lt;string&gt;</c> 欄位映射用 ValueComparer。
/// Why: string 為不可變,<c>SequenceEqual</c> 淺比對 + <c>ToList()</c> 淺拷貝即可正確偵測元素變動,
/// 不必走 <see cref="JsonValueListComparer{T}"/> 的 JSON 深拷貝路徑。
/// </summary>
public class StringListComparer : ValueComparer<List<string>>
{
    public StringListComparer()
        : base(
            (l, r) => l != null && r != null ? l.SequenceEqual(r) : l == r,
            v => v.Aggregate(0, (a, b) => HashCode.Combine(a, b.GetHashCode())),
            v => v.ToList()
        )
    { }
}

/// <summary>
/// <c>List&lt;Guid&gt;</c> 欄位映射用 ValueComparer。
/// Why: Guid 為 value type,<c>SequenceEqual</c> 淺比對 + <c>ToList()</c> 淺拷貝即可正確偵測元素變動,
/// 不必走 <see cref="JsonValueListComparer{T}"/> 的 JSON 深拷貝路徑。
/// </summary>
public class GuidListComparer : ValueComparer<List<Guid>>
{
    public GuidListComparer()
        : base(
            (l, r) => l != null && r != null ? l.SequenceEqual(r) : l == r,
            v => v.Aggregate(0, (a, b) => HashCode.Combine(a, b.GetHashCode())),
            v => v.ToList()
        )
    { }
}

/// <summary>
/// JSONB 欄位映射用 ValueComparer 共用基底:透過 JSON 序列化做深拷貝快照與值比對。
/// Why: 直接以 reference 為 Snapshot(<c>v =&gt; v</c>)會讓 ChangeTracker 在屬性 in-place 異動後
/// 仍判定值未變、SaveChanges 不發 UPDATE;序列化路徑保證 Snapshot 與當前值是兩個獨立實例。
/// </summary>
public abstract class JsonValueComparer<T> : ValueComparer<T?>
    where T : class
{
    protected JsonValueComparer()
        : base(
            (l, r) => ReferenceEquals(l, r) || (l != null && r != null && ToJson(l) == ToJson(r)),
            v => 0,
            v => v == null ? null : JsonSerializer.Deserialize<T>(ToJson(v), DbContextUtil.JsonOptions)
        )
    { }

    private static string ToJson(T v) => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions);
}

/// <summary>
/// JSONB 欄位映射用 List ValueComparer 共用基底:與 <see cref="JsonValueComparer{T}"/> 同源。
/// Why: 原本以 <c>SequenceEqual</c>+<c>ToList()</c> 對 list 做淺比對與淺拷貝,元素 in-place 異動同樣偵測不到。
/// </summary>
public abstract class JsonValueListComparer<T> : ValueComparer<List<T>>
{
    protected JsonValueListComparer()
        : base(
            (l, r) => ReferenceEquals(l, r) || (l != null && r != null && ToJson(l) == ToJson(r)),
            v => 0,
            v => v == null
                ? new List<T>()
                : JsonSerializer.Deserialize<List<T>>(ToJson(v), DbContextUtil.JsonOptions) ?? new List<T>()
        )
    { }

    private static string ToJson(List<T> v) => JsonSerializer.Serialize(v, DbContextUtil.JsonOptions);
}
