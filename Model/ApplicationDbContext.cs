namespace Model;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    #region Entities
    public DbSet<AnimalCat> AnimalCat => Set<AnimalCat>();
    public DbSet<AnimalDog> AnimalDog => Set<AnimalDog>();
    public DbSet<AppTable> AppTable => Set<AppTable>();
    public DbSet<AppIndex> AppIndex => Set<AppIndex>();
    public DbSet<ClosureNode> ClosureNode => Set<ClosureNode>();
    public DbSet<ClosurePath> ClosurePath => Set<ClosurePath>();
    public DbSet<FamilyParent> FamilyParent => Set<FamilyParent>();
    public DbSet<FamilyChild1> FamilyChild1 => Set<FamilyChild1>();
    public DbSet<FamilyChild2> FamilyChild2 => Set<FamilyChild2>();
    public DbSet<HumanHead> HumanHead => Set<HumanHead>();
    public DbSet<HumanBody> HumanBody => Set<HumanBody>();
    public DbSet<HumanLimb> HumanLimb => Set<HumanLimb>();
    public DbSet<LinkFirstContent> LinkFirstContent => Set<LinkFirstContent>();
    public DbSet<LinkFirstSubContent> LinkFirstSubContent => Set<LinkFirstSubContent>();
    public DbSet<LinkSecondContent> LinkSecondContent => Set<LinkSecondContent>();
    public DbSet<LinkSecondSubContent> LinkSecondSubContent => Set<LinkSecondSubContent>();
    public DbSet<SubjectFirst> SubjectFirst => Set<SubjectFirst>();
    public DbSet<SubjectSecond> SubjectSecond => Set<SubjectSecond>();
    public DbSet<SubjectContent> SubjectContent => Set<SubjectContent>();
    public DbSet<SubjectFirstContent> SubjectFirstContent => Set<SubjectFirstContent>();
    public DbSet<SubjectSecondContent> SubjectSecondContent => Set<SubjectSecondContent>();
    public DbSet<VehicleBase> VehicleBase => Set<VehicleBase>();
    public DbSet<VehicleSmallCar> VehicleSmallCar => Set<VehicleSmallCar>();
    public DbSet<VehicleLargeCar> VehicleLargeCar => Set<VehicleLargeCar>();
    #endregion

    #region Queries
    public virtual DbSet<KeyLessResult> KeyLessResult => Set<KeyLessResult>();
    public virtual DbSet<ViewResult> ViewResult => Set<ViewResult>();
    #endregion

    #region DbFunction
    public virtual DbSet<EmptyDbSet> EmptyDbSet => Set<EmptyDbSet>();
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

    /// <summary>
    /// 設定特定實體的配置。
    /// 這裡的設定擁有最高優先權 (High Precedence)，會覆蓋 ConfigureConventions 的預設慣例。
    /// </summary>
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

    /// <summary>
    /// 設定全域模型慣例。
    /// 注意：此處的設定為「預設值」，若在 OnModelCreating 中有特定配置，則以 OnModelCreating 為準 (Precedence)。
    /// </summary>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveColumnType(DbColumnType.TimestampWithTimeZone)
            .HaveConversion<DateTimeWithZoneConverter>();
        configurationBuilder.Properties<DateTime?>()
            .HaveColumnType(DbColumnType.TimestampWithTimeZone)
            .HaveConversion<NullableDateTimeWithZoneConverter>();

        configurationBuilder.Properties<string>()
            .HaveColumnType(DbColumnType.Text);
        configurationBuilder.Properties<string?>()
            .HaveColumnType(DbColumnType.Text);
    }

}
