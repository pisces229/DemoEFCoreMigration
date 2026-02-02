namespace Model.Scripts;

public class View
{
    public static readonly string Name = DbContextUtil.NamingConvention(nameof(View));

    //public static readonly string Skip = "-- Skip";

    public static readonly string Create = @$"
DROP VIEW IF EXISTS {Name};

CREATE OR REPLACE VIEW {Name} AS
WITH 
cte_result AS (
    SELECT id, name FROM animal_cat
)
SELECT id, name FROM cte_result;
    ";
}
