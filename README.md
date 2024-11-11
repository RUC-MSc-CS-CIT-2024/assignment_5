# assignment_5

## Question 1

```sql
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
```

## Question 2

### safe composed query function
```csharp
public void safeComposedQuery() {
    string query = "SELECT * FROM safe_course(@p_id)";

    Console.Write("Please type id of a course: ");
    string? userValue = Console.ReadLine();

    Console.WriteLine($"Query to be executed: {query}");

    client.query(query, "p_id", userValue);
}
```

### Updated do while
```csharp
do {
  Console.Write("""
    Please select character + enter"
    'd' (dynamic query)
    'c' (composed query)
    'sc' (secure composed query)
    'x' (exit)
    >
    """);
  s = Console.ReadLine()?.Trim();
  Console.WriteLine();
  switch (s) {
    case "d":
      qConstructor.dynamicQuery();
      break;
    case "c":
      qConstructor.composedQuery();
      break;
    case "sc":
      qConstructor.safeComposedQuery();
      break;
    case "x": 
      Console.WriteLine("exiting ..");
      break;
    default:
      Console.WriteLine("you typed " + "'" + s + "'" + " -- please use a suggested value");
      break;
    } // end switch
} while (s != "x");
```

## Question 3

Test input: `' AND 1=0 OR dept_name = 'Biology' --`

### S
```
Please select character + enter\n"
'd' (dynamic query)
'c' (composed query)
'sc' (secure composed query)
'x' (exit)
>c

Please type id of a course: ' AND 1=0 OR dept_name = 'Biology' --
Query to be executed: select * from course where course_id LIKE '%' AND 1=0 OR dept_name = 'Biology' --%' and dept_name != 'Biology'

 course_id | title                 | dept_name | credits
-----------+-----------------------+-----------+---------
 BIO-101   | Intro. to Biology     | Biology   | 4
 BIO-301   | Genetics              | Biology   | 4
 BIO-399   | Computational Biology | Biology   | 3
(3 rows)
```

### SC
```
Please select character + enter"
'd' (dynamic query)
'c' (composed query)
'sc' (secure composed query)
'x' (exit)
>sc

Please type id of a course: ' AND 1=0 OR dept_name = 'Biology' --
Query to be executed: SELECT * FROM safe_course(@p_id)
Query/3: SELECT * FROM safe_course(@p_id) with p_id = ' AND 1=0 OR dept_name = 'Biology' --
 course_id | title | dept_name | credits
-----------+-------+-----------+---------
(0 rows)
```

## Question 4

```csharp
public class AuthenticatorCustom : Authenticator {
    public override bool passwordIsOK(string password, string username)
    {
        if(password.Length <= 8)
            return false;
        if(username != "admin" && password.Contains(username))
            return false;

        return true;
    }
}
```

## Question 5

On my laptop its possible to run 100,000 iterations but when increasing to 1,000,000 it starts to take more than half a second.

## Question 6

The current implementation is vulnerable to SQL injection attack, when logging in the attach can specify their username as follows which would update the admin password with the attackers password:

```
' AND true; UPDATE password SET hashed_password = (SELECT hashed_password FROM password WHERE username = 'attacker'), salt = (SELECT salt FROM password WHERE username = 'attacker') WHERE username = 'admin'; --
```

This can also be done when registering a new user. This require the user to first register a user (the 'attacker') with a known password, this password hash can then be used next time to override the admin password with the attacker password.

```
dummy', 'salt', 'hash'); UPDATE password SET hashed_password = (SELECT hashed_password FROM password WHERE username = 'attacker'), salt = (SELECT salt FROM password WHERE username = 'attacker') WHERE username = 'admin'; --
```
