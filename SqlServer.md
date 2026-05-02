## [Microsoft SQL Server - Ubuntu based images](https://hub.docker.com/_/microsoft-mssql-server)

`docker pull mcr.microsoft.com/mssql/server`

`docker pull mcr.microsoft.com/mssql/server:2022-latest`

`docker pull mcr.microsoft.com/mssql/server:2019-latest`

`docker pull mcr.microsoft.com/mssql/server:2017-latest`

`docker run --name demo-mssql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=1qaz!QAZ" -p 1433:1433 -d mcr.microsoft.com/mssql/server:`

Requires the following environment flags

"ACCEPT_EULA=Y"

"MSSQL_SA_PASSWORD=<your_strong_password>"

"MSSQL_AGENT_ENABLED=true"

"MSSQL_PID=<your_product_id | edition_name> (default: Developer)"

MSSQL_PID is the Product ID (PID) or Edition that the container will run with. Acceptable values:

- Developer : This will run the container using the Developer Edition (this is the default if no MSSQL_PID environment variable is supplied)
- Express : This will run the container using the Express Edition
- Standard : This will run the container using the Standard Edition
- Enterprise : This will run the container using the Enterprise Edition
- EnterpriseCore : This will run the container using the Enterprise Edition Core

`Server=<server>;Database=<database>;User ID=<userId>;Password=<password>;`

`SELECT @@VERSION`

## [Framework Core Tool](https://docs.microsoft.com/zh-tw/ef/core/cli/powershell)

[追蹤與No-Tracking查詢](https://learn.microsoft.com/zh-tw/ef/core/querying/tracking)

### Code First

`Add-Migration <Name> -Verbose`

`Update-Database <Name> -Verbose`

`Script-Migration <From.Name> <To.Name> -Verbose`

### Db First

`Scaffold-DbContext "Server=<server>;Database=<database>;User ID=<userId>;Password=<password>;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir <path> -force`

`Scaffold-DbContext "Server=(LocalDB)\MSSQLLocalDB;Database=Demo;User ID=sa;Password=1qaz!QAZ;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir ScaffoldTemp -force`

## 資料分割資料表與索引

[0](https://docs.microsoft.com/zh-tw/sql/relational-databases/partitions/partitioned-tables-and-indexes?view=sql-server-ver16)

## SQL Server 加密 - Always Encrypted

[0](https://docs.microsoft.com/zh-tw/sql/relational-databases/security/encryption/always-encrypted-database-engine?view=sql-server-ver16)

[1](https://dotblogs.com.tw/yc421206/2019/05/18/ef6_connect_sql_server_2016_always_encrypted)

[2](https://dotblogs.com.tw/yc421206/2019/05/31/ef6_connect_sql_server_2016_always_encrypted_limit_solution)

[3](https://dotblogs.com.tw/stanley14/2016/03/19/165914)

### DB資料欄位加密

#### 手動建立主要金鑰和加密金鑰

* 資料庫 > 安全性 > Always Encrypted金鑰 > 點右鍵「新增資料行主要金鑰」
* 輸入名稱 > 選擇憑證(憑證可以自行產生) > 按確定
* 資料庫 > 安全性 > Always Encrypted金鑰 > 點右鍵「新增資料行加密金鑰」
* 輸入名稱 > 選擇資料行主要金鑰(上面新增的那個) > 按確定
* 確認金鑰添加成功

#### 手動加密欄位

* 對要加密的欄位資料表 > 點右鍵「加密資料行」
* 勾選要加密的欄位 > 選擇加密類型 > 選擇加密金鑰(預設應該是前面加的) > 下一步 > 下一步 > 下一步 > 完成
* * 隨機化：使用隨機加密值加密，欄位更安全。不允許搜尋、建立索引法當作搜索條件
* * 確定性：使用相同的加密值加密，允許搜尋、分組、建立索引
* 確認欄位已加密

### Naming conventions

Index/Constraint Type|Suffix|Naming Convention
---|---|---
Primary key(***P***rimary ***K***ey)|pk|pk__[table]
Unique index/constraint(***A***lternate ***K***ey)|uk|uk__[table]__[2-digit sequence]
Non-Unique index(***I***nde***X***)|ix|ix__[table]__[2-digit sequence]
Check constraint(***C***hec***K***)|ck|ck__[table]__[column/2-digit sequence]
Default constraint(***D***e***F***ault)|df|df__[table]__[column]
Foreign key constraint(***F***oreign ***K***ey)|fk|fk__[table]__[reference_table]

## RedLock

```
-- Declare variables
DECLARE @ReturnCode INT;

-- *** Key Step 1: Start the transaction ***
BEGIN TRANSACTION; 

-- Key Step 2: Attempt to acquire the transactional application lock (LockOwner = 'Transaction')
EXEC @ReturnCode = sp_getapplock 
    @Resource = 'DemoDistributedResource', 
    @LockMode = 'Exclusive', 
    @LockOwner = 'Transaction', 
    @LockTimeout = 3000;

IF @ReturnCode >= 0 
BEGIN
    -- Successfully acquired the lock, execute the protected business logic
    
    PRINT N'Start time: ' + CONVERT(VARCHAR(20), GETDATE(), 120);
    WAITFOR DELAY '00:00:10'; 
    PRINT N'End time: ' + CONVERT(VARCHAR(20), GETDATE(), 120);

    -- *** Key Step 3: Commit the transaction, the lock is automatically released ***
    COMMIT TRANSACTION; 
END
ELSE 
BEGIN
    -- Failed to acquire the lock, should roll back the transaction to undo all operations
    PRINT N'Failed to acquire the application lock. Return code: ' + CAST(@ReturnCode AS NVARCHAR(10));
    
    -- *** Key Step 4: Roll back the transaction ***
    ROLLBACK TRANSACTION; 
END
```
