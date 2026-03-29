namespace Model.EntityTypeConfigurations;

internal class AspUserConfiguration : IEntityTypeConfiguration<AspUser>
{
    public void Configure(EntityTypeBuilder<AspUser> builder)
    {
        builder.ToTable(DbContextUtil.NamingConvention(nameof(AspUser)));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.UserName).HasMaxLength(256);
        builder.Property(e => e.NormalizedUserName).HasMaxLength(256);
        builder.Property(e => e.Email).HasMaxLength(256);
        builder.Property(e => e.NormalizedEmail).HasMaxLength(256);
        builder.Property(e => e.PhoneNumber).HasMaxLength(50);
        builder.Property(e => e.ConcurrencyStamp).HasMaxLength(256);
        builder.Property(e => e.SecurityStamp).HasMaxLength(256);
        builder.Property(e => e.PasswordHash).HasMaxLength(512);

        builder.Property(e => e.Description).HasMaxLength(128);

        builder.HasMany<AspUserClaim>()
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder.HasMany<AspUserLogin>()
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder.HasMany<AspUserToken>()
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder.HasMany<AspUserRole>()
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired();
    }
}
