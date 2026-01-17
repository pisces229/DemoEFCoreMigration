using Model.Queries;

namespace Model.Entities;

public class AnimalCat : Animal
{
    public double WhiskerLength { get; set; }
    public bool LovesBox { get; set; }

    public virtual ViewResult ViewResult { get; set; } = null!;
    public virtual FuncTableResult FuncTableResult { get; set; } = null!;
}
