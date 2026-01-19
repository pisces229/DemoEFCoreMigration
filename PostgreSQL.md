## [PostgreSQL](https://www.postgresql.org/)

## [PostgreSQL Docker](https://hub.docker.com/_/postgres)

`docker pull postgres`

`docker run -d -p 5432:5432 --name demo-postgres -e POSTGRES_PASSWORD=Aa123456 postgres:18`

`docker run --name demo-postgres -e POSTGRES_PASSWORD=Aa123456 -p 5432:5432 -d postgres:18`

`docker run --name demo-postgres -e POSTGRES_PASSWORD=Aa123456 -p 5432:5432 -v demo-postgres-data:/var/lib/postgresql/data -d postgres:18`

`docker run --name demo-postgres -e POSTGRES_PASSWORD=Aa123456 -p 5432:5432 -v "D:\docker\pgdata:/var/lib/postgresql/data" -d postgres:18`

```bash
docker inspect demo-postgres | grep Mounts -A 5
docker inspect demo-postgres | Select-String "Mounts" -Context 0,5
docker inspect -f "{{json .Mounts}}" demo-postgres
```

`Host=<host>;Database=<database>;Username=<username>;Password=<password>;`

`SELECT VERSION()`

## management tools

[pgAdmin](https://www.pgadmin.org/)

## [Framework Core Tool](https://docs.microsoft.com/zh-tw/ef/core/cli/powershell)

### Code First

`Add-Migration <Name> -Verbose`

`Update-Database <Name> -Verbose`

`Script-Migration <From.Name> <To.Name> -Verbose`

```bash
dotnet tool install --global dotnet-ef

$env:Path += ";$env:USERPROFILE\.dotnet\tools"

dotnet ef migrations add <migration_id> --project Model/Model.csproj --startup-project DbMigration --context ApplicationDbContext --output-dir Migrations -v

dotnet ef migrations add Init --project Model/Model.csproj --startup-project DbMigration --context ApplicationDbContext --output-dir Migrations -v

$env:DbContext = "Host=localhost;Database=demo;Username=postgres;Password=Aa123456;"

# This command adjusts the database to the latest migration state.
dotnet ef database update --project Model --startup-project DbMigration --context ApplicationDbContext -v

# This command adjusts the database to the specified migration state.
dotnet ef database update <migration_id> --project Model --startup-project DbMigration --context ApplicationDbContext -v

# Delete the migration in the code and revert the snapshot.
dotnet ef migrations remove --project Model --startup-project DbMigration --context ApplicationDbContext -v
```

### Db First

`Scaffold-DbContext "Host=<host>;Database=<database>;Username=<username>;Password=<password>;" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir <path> -force`

`Scaffold-DbContext "Host=localhost;Database=Demo;Username=postgres;Password=1qaz!QAZ;" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir ScaffoldTemp -force`


## [pgadmin4](https://hub.docker.com/r/dpage/pgadmin4/)

`docker run -d -p 5050:80 -e "PGADMIN_DEFAULT_EMAIL=test@mail.com" -e "PGADMIN_DEFAULT_PASSWORD=1qaz!QAZ" dpage/pgadmin4`

## docker compose
```yaml
version: '3.9'

services:
  postgres:
    image: postgres:16
    container_name: postgres
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: secret
      POSTGRES_DB: mydb
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  pgadmin:
    image: dpage/pgadmin4
    container_name: pgadmin
    environment:
      PGADMIN_DEFAULT_EMAIL: admin@example.com
      PGADMIN_DEFAULT_PASSWORD: admin123
    ports:
      - "8080:80"
    depends_on:
      - postgres

  pgagent:
    image: dpage/pgagent
    container_name: pgagent
    environment:
      PGHOST: postgres
      PGPORT: 5432
      PGUSER: postgres
      PGPASSWORD: secret
      PGDATABASE: mydb
    depends_on:
      - postgres
    restart: unless-stopped

volumes:
  postgres_data:
```

## RedLock
```
DO $$
DECLARE
    resource_id BIGINT := 99999;
    lock_acquired BOOLEAN;
BEGIN
    -- *** 1. Corrected Function: pg_try_advisory_xact_lock(bigint) ***
    -- Attempts to acquire a non-blocking, transaction-level lock bound to the current transaction.
    SELECT pg_try_advisory_xact_lock(resource_id) INTO lock_acquired;

    IF lock_acquired THEN
        RAISE NOTICE 'Successfully acquired lock (ID: %).', resource_id;
        
        -- Execute protected business logic
        -- Simulate long-running process (e.g., 10 seconds)
        PERFORM pg_sleep(10); 
        
        RAISE NOTICE 'Business logic completed. Preparing to commit transaction, lock will be automatically released.';
        
        -- COMMIT/ROLLBACK happens externally 
        
    ELSE
        RAISE NOTICE 'Failed to acquire lock (ID: %), resource is busy.', resource_id;
        
        -- Execute fallback logic (e.g., retry or log error)
        
    END IF;
END $$;
```

## Zone

```sql
SET TIME ZONE 'UTC';
SET TIME ZONE 'Asia/Taipei';

SELECT
	CURRENT_TIMESTAMP AS "timestamp with time zone",
	LOCALTIMESTAMP AS "timestamp without time zone"
```

### timestamp without time zone (手動管理)

* 連線字串: Timezone=Asia/Taipei; (強烈建議)
* 資料庫預設值: LOCALTIMESTAMP
* EF Core Converter (存): v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified)
* EF Core Converter (取): v => DateTime.SpecifyKind(v, DateTimeKind.Local)
* 對應 C# 類型: DateTime (Unspecified)
* 適用場景: 單一時區、舊資料庫、鬧鐘排班系統

### timestamp with time zone (資料庫管理)

* 連線字串: Timezone=UTC; (業界標準)
* 資料庫預設值: now() 或 CURRENT_TIMESTAMP
* EF Core Converter (存): v => v.ToUniversalTime() (確保為 UTC)
* EF Core Converter (取): 不需要 (Npgsql 預設讀出即為 UTC)
* 對應 C# 類型: DateTimeOffset 或 DateTime (Utc)
* 適用場景: 新專案首選、跨國系統、精確事件記錄

## 安裝 plpgsql_check 靜態程式碼分析

### 在 Ubuntu / Debian 安裝

```bash
sudo apt-get update
sudo apt-get install postgresql-18-plpgsql-check
```

### 啟用 shared_preload_libraries
```conf
# edit /etc/postgresql/18/main/postgresql.conf
shared_preload_libraries = 'plpgsql_check'
plpgsql_check.show_performance_warnings = on
plpgsql_check.enable_tracer = on
```

```bash
sudo systemctl restart postgresql
```

```sql
CREATE EXTENSION IF NOT EXISTS plpgsql_check;
SELECT * FROM pg_extension WHERE extname = 'plpgsql_check';
SHOW shared_preload_libraries;
```

### 在 Docker 環境安裝

```Dockerfile
FROM postgres:18

RUN apt-get update && apt-get install -y postgresql-18-plpgsql-check && rm -rf /var/lib/apt/lists/*
```

`docker build -f Dockerfile.plpgsql-check -t demo-plpgsql-check:dev .`

`docker run -d -p 5432:5432 --name demo-postgres -e POSTGRES_PASSWORD=Aa123456 demo-plpgsql-check:dev`

### 在資料庫中啟用 (必須執行)

```sql
CREATE EXTENSION IF NOT EXISTS plpgsql_check;

SELECT * FROM pg_extension WHERE extname = 'plpgsql_check';
```

### 如何使用 (檢查範例)

```sql
SELECT * FROM plpgsql_check_function_tb('your_function_name(arg1_type, arg2_type)');
```

### 所有 PL/pgSQL 函數並回報錯誤

```sql
SELECT 
    p.proname AS function_name,
    check_res.*
FROM 
    pg_proc p
JOIN 
    pg_namespace n ON p.pronamespace = n.oid
CROSS JOIN LATERAL 
    plpgsql_check_function_tb(p.oid) AS check_res
--LEFT CROSS JOIN LATERAL 
--    plpgsql_check_function_tb(p.oid) AS check_res ON TRUE
WHERE 
    n.nspname NOT IN ('pg_catalog', 'information_schema')
    AND p.prolang = (SELECT oid FROM pg_language WHERE lanname = 'plpgsql');
```

```sql
DO $$
DECLARE 
    error_details text;
BEGIN
    -- 1. Identify and check ONLY PL/pgSQL functions
    -- Filters by lanname = 'plpgsql' to avoid ""not a plpgsql function"" errors
    SELECT 
        'Function: ' || n.nspname || '.' || p.proname || ' | Message: ' || c.message 
    INTO error_details
    FROM pg_proc p
    JOIN pg_namespace n ON p.pronamespace = n.oid
    JOIN pg_language l ON p.prolang = l.oid -- Join with pg_language
    CROSS JOIN LATERAL plpgsql_check_function_tb(p.oid) c
    WHERE n.nspname NOT IN ('pg_catalog', 'information_schema') 
        AND l.lanname = 'plpgsql' -- CRITICAL: Only check PL/pgSQL functions
        AND c.level = 'error'
    LIMIT 1;

    -- 2. If an error is detected, raise an exception to trigger a transaction rollback
    IF error_details IS NOT NULL THEN
        RAISE EXCEPTION 'PL/pgSQL validation failed! Details => %', error_details;
    END IF;

    RAISE NOTICE 'All PL/pgSQL functions passed validation.';
END $$;
```

## DROP CONSTRAINT

```
ALTER TABLE <table> DROP CONSTRAINT IF EXISTS <constraint>;
```
