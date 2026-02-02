namespace Model.Queries;

public record FuncTableWithParamInput(long Id);

public class FuncTableWithParamResult
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
