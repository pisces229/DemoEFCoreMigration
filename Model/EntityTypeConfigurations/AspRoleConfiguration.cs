namespace Model.EntityTypeConfigurations;

internal class AspRoleConfiguration : IEntityTypeConfiguration<AspRole>
{
    public void Configure(EntityTypeBuilder<AspRole> builder)
    {
        builder.ToTable(DbContextUtil.NamingConvention(nameof(AspRole)));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Name).HasMaxLength(256);
        builder.Property(e => e.NormalizedName).HasMaxLength(256);
        builder.Property(e => e.ConcurrencyStamp).HasMaxLength(256);

        builder.Property(e => e.Description).HasMaxLength(128);

        builder.HasMany<AspRoleClaim>()
            .WithOne()
            .HasForeignKey(e => e.RoleId)
            .IsRequired();

        builder.HasMany<AspUserRole>()
            .WithOne()
            .HasForeignKey(e => e.RoleId)
            .IsRequired();
    }
}
