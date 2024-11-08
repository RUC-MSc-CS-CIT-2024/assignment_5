CREATE OR REPLACE FUNCTION safe_course(p_id VARCHAR(8))
RETURNS TABLE(
    course_id VARCHAR,
    title VARCHAR,
    dept_name VARCHAR,
    credits numeric(2, 0)
) 
AS $$
    SELECT * 
    FROM course 
    WHERE course_id = p_id 
        AND dept_name != 'Biology';
$$ LANGUAGE sql;
