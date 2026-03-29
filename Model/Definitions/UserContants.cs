namespace Model.Definitions;

public class UserContants
{
    public static class System
    {
        public static readonly Guid Id = Guid.Parse("00000000-0000-7000-8000-000000000000");
        public static readonly string Name = "system";
    }
    public static class Admin
    {
        public static readonly Guid Id = Guid.Parse("00000000-0000-7000-8000-000000000001");
        public static readonly string Name = "admin";
    }
}
