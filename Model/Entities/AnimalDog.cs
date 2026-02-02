namespace Model.Entities;

public class AnimalDog : Animal
{
    public string Breed { get; set; } = string.Empty;
    public bool IsGoodBoy { get; set; }
}
