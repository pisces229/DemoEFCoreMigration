namespace Model.EntityTypeConfigurations;

internal class AspRoleClaimConfiguration : IEntityTypeConfiguration<AspRoleClaim>
{
    public void Configure(EntityTypeBuilder<AspRoleClaim> builder)
    {
        builder.ToTable(DbContextUtil.NamingConvention(nameof(AspRoleClaim)));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ClaimType).HasMaxLength(256);
        builder.Property(e => e.ClaimValue).HasMaxLength(1024);
    }
}
