namespace Model.Scripts;

public class FuncScalarWithParam
{
    public static readonly string Name = DbContextUtil.NamingConvention(nameof(FuncScalarWithParam));

    //public static readonly string Skip = "-- Skip";

    public static readonly string Create = @$"
DROP FUNCTION IF EXISTS {Name}(BIGINT);

CREATE OR REPLACE FUNCTION {Name}(value BIGINT)
RETURNS BIGINT
LANGUAGE plpgsql
AS $$
#variable_conflict use_column
BEGIN
    RETURN value * (10);
END;
$$;
    ";
}
