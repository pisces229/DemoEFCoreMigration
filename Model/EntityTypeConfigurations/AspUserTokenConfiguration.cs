namespace Model.EntityTypeConfigurations;

internal class AspUserTokenConfiguration : IEntityTypeConfiguration<AspUserToken>
{
    public void Configure(EntityTypeBuilder<AspUserToken> builder)
    {
        builder.ToTable(DbContextUtil.NamingConvention(nameof(AspUserToken)));

        builder.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

        builder.Property(e => e.LoginProvider).HasMaxLength(256);
        builder.Property(e => e.Name).HasMaxLength(256);
        builder.Property(e => e.Value).HasMaxLength(2048);
    }
}
