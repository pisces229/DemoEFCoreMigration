using Model.Definitions;

namespace Model.Entities;

public abstract class Animal
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Flag Flag { get; set; }
}
