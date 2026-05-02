using Microsoft.AspNetCore.Identity;

namespace Model.Entities;

public class AspRole : IdentityRole<Guid>
{
    public override Guid Id { get; set; } = Guid.CreateVersion7();
    public string Description { get; set; } = string.Empty;
}
