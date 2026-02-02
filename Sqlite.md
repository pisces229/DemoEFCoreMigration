## [Framework Core Tool](https://docs.microsoft.com/zh-tw/ef/core/cli/powershell)

### Code First

`Add-Migration <Name> -Verbose`

`Update-Database <Name> -Verbose`

`Script-Migration <From.Name> <To.Name> -Verbose`

### Db First

`Scaffold-DbContext "Data Source=<dataSource>;" Microsoft.EntityFrameworkCore.Sqlite -OutputDir <path>`

`Scaffold-DbContext "Data Source=c:/workspace/Database/SQLite/Demo.db;" Microsoft.EntityFrameworkCore.Sqlite -OutputDir ScaffoldTemp`

