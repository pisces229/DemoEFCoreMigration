namespace Model.Queries;

public record FuncTableWithParamInput(Guid Id);

public class FuncTableWithParamResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
