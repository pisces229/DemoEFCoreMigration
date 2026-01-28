namespace Model;

public class DbMaintenanceScript
{
    /// <summary>
    /// Drop Constraint Script
    /// </summary>
    public static readonly string DropConstraintScript = @$"
        DO $$ 
        DECLARE 
            r RECORD;
        BEGIN
            FOR r IN (
                SELECT 
                    conname, 
                    conrelid::regclass AS relname
                FROM 
                    pg_constraint 
                WHERE 
                    contype = 'f' 
                    AND conname ILIKE 'd_%'
            ) LOOP
                EXECUTE 'ALTER TABLE ' || r.relname || ' DROP CONSTRAINT ' || quote_ident(r.conname);
                RAISE NOTICE 'Dropped constraint % from table %', r.conname, r.relname;
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
