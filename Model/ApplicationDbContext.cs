using Microsoft.EntityFrameworkCore;
using Model.Definitions;
using Model.Entities;
using Model.Queries;
using System.Reflection;

namespace Model;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    #region Entities
    public DbSet<AnimalCat> AnimalCat { get; set; }
    public DbSet<AnimalDog> AnimalDog { get; set; }
    public DbSet<AppTable> AppTable { get; set; }
    public DbSet<AppIndex> AppIndex { get; set; }
    public DbSet<HumanHead> HumanHead { get; set; }
    public DbSet<HumanBody> HumanBody { get; set; }
    public DbSet<HumanLimb> HumanLimb { get; set; }
    public DbSet<LinkFirstContent> LinkFirstContent { get; set; }
    public DbSet<LinkFirstSubContent> LinkFirstSubContent { get; set; }
    public DbSet<LinkSecondContent> LinkSecondContent { get; set; }
    public DbSet<LinkSecondSubContent> LinkSecondSubContent { get; set; }
    public DbSet<SubjectFirst> SubjectFirst { get; set; }
    public DbSet<SubjectSecond> SubjectSecond { get; set; }
    public DbSet<SubjectContent> SubjectContent { get; set; }
    public DbSet<SubjectFirstContent> SubjectFirstContent { get; set; }
    public DbSet<SubjectSecondContent> SubjectSecondContent { get; set; }
    public DbSet<TablePerHierarchy> TablePerHierarchyTables { get; set; }
    #endregion

    #region Queries
    public virtual DbSet<KeyLessResult> KeyLessResult { get; set; }
    public virtual DbSet<ViewResult> ViewResult { get; set; }
    #endregion

    #region DbFunction
    public virtual DbSet<EmptyDbSet> EmptyDbSet { get; set; }
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
    public int FuncScalarWithParam(FuncScalarWithParamInput input) => FuncScalarWithParam(input.Id);
    public int FuncScalarWithParam(long id) => throw new NotSupportedException();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

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
