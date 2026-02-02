namespace Model.Scripts;

public class FuncScalar
{
    public static readonly string Name = DbContextUtil.NamingConvention(nameof(FuncScalar));

    //public static readonly string Skip = "-- Skip";

    public static readonly string Create = @$"
DROP FUNCTION IF EXISTS {Name}();

CREATE OR REPLACE FUNCTION {Name}()
RETURNS BIGINT
LANGUAGE plpgsql
AS $$
#variable_conflict use_column
BEGIN
    RETURN (10) * (10);
END;
$$;
    ";
}
