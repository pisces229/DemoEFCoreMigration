namespace Model.QueryTypeConfigurations;

public class EmptyDbSetTypeConfiguration : IEntityTypeConfiguration<EmptyDbSet>
{
    public void Configure(EntityTypeBuilder<EmptyDbSet> builder)
    {
        builder.ToTable(t => t.ExcludeFromMigrations());

        builder.HasNoKey();
    }
}
