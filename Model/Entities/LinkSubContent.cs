namespace Model.Entities;

public abstract class LinkSubContent
{
    public long Id { get; set; }
    public string Content { get; set; } = null!;
}

public class LinkFirstSubContent : LinkSubContent
{
    public string First { get; set; } = null!;
}

public class LinkSecondSubContent : LinkSubContent
{
    public string Second { get; set; } = null!;
}
