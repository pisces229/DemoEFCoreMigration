namespace Model;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    #region Entities
    public DbSet<AnimalCat> AnimalCat { get; set; } = default!;
    public DbSet<AnimalDog> AnimalDog { get; set; } = default!;
    public DbSet<AppTable> AppTable { get; set; } = default!;
    public DbSet<AppIndex> AppIndex { get; set; } = default!;
    public DbSet<ClosureNode> ClosureNode { get; set; } = default!;
    public DbSet<ClosurePath> ClosurePath { get; set; } = default!;
    public DbSet<FamilyParent> FamilyParent { get; set; } = default!;
    public DbSet<FamilyChild1> FamilyChild1 { get; set; } = default!;
    public DbSet<FamilyChild2> FamilyChild2 { get; set; } = default!;
    public DbSet<HumanHead> HumanHead { get; set; } = default!;
    public DbSet<HumanBody> HumanBody { get; set; } = default!;
    public DbSet<HumanLimb> HumanLimb { get; set; } = default!;
    public DbSet<LinkFirstContent> LinkFirstContent { get; set; } = default!;
    public DbSet<LinkFirstSubContent> LinkFirstSubContent { get; set; } = default!;
    public DbSet<LinkSecondContent> LinkSecondContent { get; set; } = default!;
    public DbSet<LinkSecondSubContent> LinkSecondSubContent { get; set; } = default!;
    public DbSet<SubjectFirst> SubjectFirst { get; set; } = default!;
    public DbSet<SubjectSecond> SubjectSecond { get; set; } = default!;
    public DbSet<SubjectContent> SubjectContent { get; set; } = default!;
    public DbSet<SubjectFirstContent> SubjectFirstContent { get; set; } = default!;
    public DbSet<SubjectSecondContent> SubjectSecondContent { get; set; } = default!;
    public DbSet<VehicleBase> VehicleBase { get; set; } = default!;
    public DbSet<VehicleSmallCar> VehicleSmallCar { get; set; } = default!;
    public DbSet<VehicleLargeCar> VehicleLargeCar { get; set; } = default!;
    #endregion

    #region Queries
    public virtual DbSet<KeyLessResult> KeyLessResult { get; set; } = default!;
    public virtual DbSet<ViewResult> ViewResult { get; set; } = default!;
    #endregion

    #region DbFunction
    public virtual DbSet<EmptyDbSet> EmptyDbSet { get; set; } = default!;
    /// <summary>
    /// FuncTable
    /// </summary>
    public IQueryable<FuncTableResult> FuncTable() => FromExpression(() => FuncTable());
    /// <summary>
    /// FuncTableWithParam
    /// </summary>
    public IQueryable<FuncTableWithParamResult> FuncTableWithParam(FuncTableWithParamInput input) => FuncTableWithParam(input.Id);
    public IQueryable<FuncTableWithParamResult> FuncTableWithParam(long id) => FromExpression(() => FuncTableWithParam(id));
    /// <summary>
    /// FuncScalar
    /// </summary>
    public int FuncScalar() => throw new NotSupportedException();
    /// <summary>
    /// FuncScalarWithParam
    /// </summary>
    public int FuncScalarWithParam(long id) => throw new NotSupportedException();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.IsKeyless || string.IsNullOrWhiteSpace(entity.GetTableName()))
            {
                continue;
            }
            //Console.WriteLine($"DefaultTableName: {entity.GetDefaultTableName()}");
            //Console.WriteLine($"TableName: {entity.GetTableName()}");
            if (entity.GetTableName()!.Length > DbContextUtil.MaxTableNameLength)
            {
                throw new InvalidOperationException($"Table name '{entity.GetTableName()}' exceeds maximum length of {DbContextUtil.MaxTableNameLength} characters.");
            }
            foreach (var property in entity.GetProperties())
            {
                //Console.WriteLine($"DefaultColumnName: {property.GetDefaultColumnName()}");
                //Console.WriteLine($"ColumnName: {property.GetColumnName()}");
                if (property.GetColumnName()!.Length > DbContextUtil.MaxNameLength)
                {
                    throw new InvalidOperationException($"Column name '{property.GetColumnName()}' in table '{entity.GetTableName()}' exceeds maximum length of {DbContextUtil.MaxNameLength} characters.");
                }
            }
            foreach (var mutableKey in entity.GetKeys())
            {
                DbContextUtil.HashKeyName(mutableKey);
            }
            //foreach (var mutableCheckConstraint in entity.GetCheckConstraints())
            //{
            //    DbContextUtil.HashCheckConstraintName(mutableCheckConstraint);
            //}
            foreach (var mutableIndex in entity.GetIndexes())
            {
                DbContextUtil.HashDatabaseName(mutableIndex);
            }
            foreach (var mutableForeignKey in entity.GetForeignKeys())
            {
                DbContextUtil.HashConstraintName(mutableForeignKey);
            }
        }

        modelBuilder.RegisterFunctions();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveColumnType(DbColumnType.TimestampWithTimeZone)
            .HaveConversion<DateTimeWithZoneConverter>();
        configurationBuilder.Properties<DateTime?>()
            .HaveColumnType(DbColumnType.TimestampWithTimeZone)
            .HaveConversion<NullableDateTimeWithZoneConverter>();
    }

}
