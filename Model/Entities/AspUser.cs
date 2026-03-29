using Microsoft.AspNetCore.Identity;

namespace Model.Entities;

public class AspUser : IdentityUser<Guid>
{
    public override Guid Id { get; set; } = Guid.CreateVersion7();
    public override string? SecurityStamp { get; set; } = Guid.CreateVersion7().ToString();
    public string Description { get; set; } = string.Empty;
}
