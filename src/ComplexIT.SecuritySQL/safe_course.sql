CREATE OR REPLACE FUNCTION safe_course(type VARCHAR(8))
RETURNS TABLE (
    course_id VARCHAR(5),
    title VARCHAR(50),
    dept_name VARCHAR(20),
    credits NUMERIC(2,0)
)
LANGUAGE sql AS
$$
SELECT course_id, title, dept_name, credits
FROM course
WHERE course.course_id = type
AND dept_name != 'Biology';
$$;


SELECT * FROM safe_course('CS-101');

## ' OR true --