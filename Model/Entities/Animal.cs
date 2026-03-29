namespace Model.Entities;

public abstract class Animal
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Flag Flag { get; set; }
}
