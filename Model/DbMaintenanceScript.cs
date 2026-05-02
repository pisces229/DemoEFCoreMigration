namespace Model;

public class DbMaintenanceScript
{
    /// <summary>
    /// Drop dummy <c>d_</c>-prefixed FK constraints. Scoped to the connection's
    /// <c>current_schema()</c>; caller must ensure the connection's
    /// <c>search_path</c> resolves to the target schema (production = default
    /// <c>public</c>; tests set <c>SearchPath</c> per schema).
    /// </summary>
    public static readonly string DropConstraintScript = @"
        DO $$
        DECLARE
            r RECORD;
        BEGIN
            FOR r IN (
                SELECT
                    n.nspname AS schema_name,
                    cl.relname AS table_name,
                    c.conname  AS constraint_name
                FROM pg_constraint c
                JOIN pg_namespace n  ON c.connamespace = n.oid
                JOIN pg_class    cl ON c.conrelid     = cl.oid
                WHERE c.contype = 'f'
                  AND c.conname ILIKE 'd\_%' ESCAPE '\'
                  AND n.nspname = current_schema()
            ) LOOP
                EXECUTE format('ALTER TABLE %I.%I DROP CONSTRAINT IF EXISTS %I',
                               r.schema_name, r.table_name, r.constraint_name);
                RAISE NOTICE 'Dropped constraint % from table %.%',
                    r.constraint_name, r.schema_name, r.table_name;
            END LOOP;
        END $$;
    ";
    /// <summary>
    /// CREATE EXTENSION plpgsql_check
    /// </summary>
    public const string CreatePlpgsqlCheckScript = @"
        CREATE EXTENSION IF NOT EXISTS plpgsql_check;
    ";
    /// <summary>
    /// Plpgsql Check Script
    /// </summary>
    public const string ExcutePlpgsqlCheckScript = @"
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
    ";
}
