namespace Model.EntityTypeConfigurations;

internal class AspUserLoginConfiguration : IEntityTypeConfiguration<AspUserLogin>
{
    public void Configure(EntityTypeBuilder<AspUserLogin> builder)
    {
        builder.ToTable(DbContextUtil.NamingConvention(nameof(AspUserLogin)));

        builder.HasKey(e => new { e.LoginProvider, e.ProviderKey });

        builder.Property(e => e.LoginProvider).HasMaxLength(256);
        builder.Property(e => e.ProviderKey).HasMaxLength(256);
        builder.Property(e => e.ProviderDisplayName).HasMaxLength(256);
    }
}
