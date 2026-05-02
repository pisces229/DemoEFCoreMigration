namespace Model.Scripts;

public class ProcWithParam
{
    public static readonly string Create = @$"
DROP PROCEDURE IF EXISTS {Proc.Name}(INT, TEXT);

CREATE OR REPLACE PROCEDURE {Proc.Name}(IN p_id INT, INOUT p_name TEXT)
LANGUAGE plpgsql
AS $$
BEGIN
    p_name := 'id-' || p_id;
END;
$$;
    ";
}
