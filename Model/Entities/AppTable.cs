using Model.Definitions;
using Model.JsonObjects;

namespace Model.Entities;

public partial class AppTable
{
    public long Id { get; set; }
    public DataType? Enum { get; set; }
    public string? String { get; set; } = null!;
    public int? Int { get; set; }
    public long? Long { get; set; }
    public decimal? Decimal { get; set; }
    public DateOnly? DateOnly { get; set; }
    public DateTime? DateTime { get; set; }
    public DateTimeOffset? DateTimeOffset { get; set; }
    public IEnumerable<string> StringJsonObjects { get; set; } = [];
    public ValueJsonObject? ValueJsonObject { get; set; }
    public IEnumerable<ValueJsonObject> ValueJsonObjects { get; set; } = [];
    /// <summary>
    /// Sql Server RowVersion
    /// </summary>
    //public byte[] RowVersion { get; set; } = null!;
    /// <summary>
    /// PostgreSQL RowVersion
    /// </summary>
    public uint RowVersion { get; set; }
}
