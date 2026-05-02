namespace Model.Scripts;

public class FuncTable
{
    public static readonly string Name = DbContextUtil.NamingConvention(nameof(FuncTable));

    //public static readonly string Skip = "-- Skip";

    public static readonly string Create = @$"
DROP FUNCTION IF EXISTS {Name}();

CREATE OR REPLACE FUNCTION {Name}()
RETURNS TABLE (
    id UUID,
    name TEXT
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
