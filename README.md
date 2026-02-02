# DemoEFCoreMigration

[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

[Design-time DbContext Creation](https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli)

`docker run --name demo-mssql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Aa123456" -p 1433:1433 -d mcr.microsoft.com/mssql/server`

`Server=localhost;Database=Demo;User ID=sa;Password=Aa123456;TrustServerCertificate=True;`

```
dotnet tool install --global dotnet-ef

dotnet ef migrations add Initialize --project Model/Model.csproj --startup-project DbMigration --context ApplicationDbContext --output-dir Migrations -v
```

```
dotnet ef migrations add {Description} --project {Project} --startup-project {StartupProject} --context {Context} --output-dir {OutputDir} -v

dotnet ef migrations list --project {Project} --startup-project {StartupProject} --context {Context} -v

dotnet ef database update {migration} --project {Project} --startup-project {StartupProject} --context {Context} -v

dotnet ef migrations remove --project {Project} --startup-project {StartupProject} --context {Context} -v
```

```
# DbContext: SqlServer, PostgreSQL, Sqlite
$env:DbContext="DbContext"

dotnet ef migrations add Create --project Model/Model.csproj --startup-project DbMigration --context ApplicationDbContext --output-dir Migrations -v

dotnet ef migrations add Modify --project Model/Model.csproj --startup-project DbMigration --context ApplicationDbContext --output-dir Migrations -v

dotnet ef migrations list --project Model/Model.csproj --startup-project DbMigration --context ApplicationDbContext -v

$env:DbContext = "Server=localhost;Database=Demo;User ID=sa;Password=Aa123456;TrustServerCertificate=True;"

# This command adjusts the database to the latest migration state.
dotnet ef database update --project Model --startup-project DbMigration --context ApplicationDbContext -v

# This command adjusts the database to the specified migration state.
dotnet ef database update 20240715155917_Create --project Model --startup-project DbMigration --context ApplicationDbContext -v

# Delete the migration in the code and revert the snapshot.
dotnet ef migrations remove --project Model --startup-project DbMigration --context ApplicationDbContext -v
```

## DeleteBehavior

* Cascade (CASCADE)
* ClientCascade (NO ACTION)
```
DELETE FROM [HumanLimb]
OUTPUT 1
WHERE [Row] = @p0;
DELETE FROM [HumanBody]
OUTPUT 1
WHERE [Row] = @p1;
```

* Restrict (NO ACTION)
* SetNull (SET NULL)
* ClientSetNull (NO ACTION)
* NoAction (NO ACTION)
```
UPDATE [HumanLimb] SET [BodyId] = @p0
OUTPUT 1
WHERE [Row] = @p1;
DELETE FROM [HumanBody]
OUTPUT 1
WHERE [Row] = @p2;
```

* ClientNoAction (NO ACTION)
```
DELETE FROM [HumanBody]
OUTPUT 1
WHERE [Row] = @p0;
```

## [Inheritance](https://learn.microsoft.com/en-us/ef/core/modeling/inheritance)

# TPH (Table Per Hierarchy)
> Maps an entire inheritance hierarchy to a single database table, using a discriminator column to identify specific types. The advantage is higher performance and simpler structure; the disadvantage is that the table structure can become very wide, containing many NULL values.

# TPT (Table Per Type)
> Maps each specific type to a separate database table, with the base class properties stored in the base table and the derived class properties stored in derived class tables, linked by foreign keys. The advantage is a more compact table structure without extra NULL values; the disadvantage is lower performance and more complex structure.

# TPC (Table Per Concrete Class)
> Maps each specific type to a separate database table, with each table containing all properties, including those of the base class, without foreign key relationships between tables. The advantage is higher performance and a more compact table structure; the disadvantage is data redundancy, as the base class properties are repeated in each table.
