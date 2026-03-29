namespace Model.EntityTypeConfigurations;

internal class AspUserRoleConfiguration : IEntityTypeConfiguration<AspUserRole>
{
    public void Configure(EntityTypeBuilder<AspUserRole> builder)
    {
        builder.ToTable(DbContextUtil.NamingConvention(nameof(AspUserRole)));

        builder.HasKey(e => new { e.UserId, e.RoleId });
    }
}
