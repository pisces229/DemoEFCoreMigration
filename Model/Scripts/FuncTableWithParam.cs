namespace Model.Scripts;

public class FuncTableWithParam
{
    public static readonly string Name = DbContextUtil.NamingConvention(nameof(FuncTableWithParam));

    //public static readonly string Skip = "-- Skip";

    public static readonly string Create = @$"
DROP FUNCTION IF EXISTS {Name}(BIGINT);

CREATE OR REPLACE FUNCTION {Name}(value BIGINT)
RETURNS TABLE (
    id BIGINT,
    name VARCHAR(100)
) 
LANGUAGE plpgsql
AS $$
#variable_conflict use_column
BEGIN
    RETURN QUERY
    WITH
    cte_result AS (
        SELECT id, name FROM animal_cat
    )
    SELECT id, name FROM cte_result;
END;
$$;
    ";
}
