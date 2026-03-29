namespace Model.EntityTypeConfigurations;

internal class AspUserClaimConfiguration : IEntityTypeConfiguration<AspUserClaim>
{
    public void Configure(EntityTypeBuilder<AspUserClaim> builder)
    {
        builder.ToTable(DbContextUtil.NamingConvention(nameof(AspUserClaim)));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ClaimType).HasMaxLength(256);
        builder.Property(e => e.ClaimValue).HasMaxLength(1024);
    }
}
