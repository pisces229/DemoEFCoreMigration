namespace Model.Scripts;

public class Proc
{
    public static readonly string Name = DbContextUtil.NamingConvention(nameof(Proc));

    //public static readonly string Skip = "-- Skip";

    public static readonly string Create = @$"
DROP PROCEDURE IF EXISTS {Name};

CREATE OR REPLACE PROCEDURE {Name}()
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE animal_cat SET birth_date = NOW();
END;
$$;
    ";
}
