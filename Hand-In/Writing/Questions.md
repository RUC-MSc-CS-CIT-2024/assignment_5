# Assignment 5 - Security
---
## Complex IT Systems - Fall 2024

Group **cit11**: 
- Ida Hay Jørgensen (stud-ijoergense@ruc.dk)
- Julius Krüger Madsen (stud-juliusm@ruc.dk)
- Marek Laslo (stud-laslo@ruc.dk)
- Sofus Hilfling Nielsen (stud-sofusn@ruc.dk)
---
> NOTE : Questions 1-3 are about SQL Injection.
---  

### Question 1. 
> *Define a stored database function that returns a table of courses from the course table in the university database. You may name the stored function safe_course(). The function should have one input parameter of type VARCHAR(8). The function should return courses whose course id match the input. (There will be at most one such course, since the course id is a primary key of the table). Naturally, the function should leave out any course offered by the Biology department. Show the SQL code that defines the function.*  

This function, `safe_course`, aims to return specific course information while excluding those from the Biology department. By using parameterized input (`p_id` of type `VARCHAR(8)`), it avoids potential SQL injection risks. The structure is as follows:  

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

> NOTE: safe_course(p_id) function in postgres DB university  

This function takes a `course_id` as input and returns a row from the course table if it exists, filtering out Biology courses by specifying `AND c.dept_name != 'Biology'`. This approach not only mitigates SQL injection by handling inputs safely but also ensures only courses from other departments are returned.  

### Question 2. 
> *In SQL-Injection-Frontend, in the switch statement of Program.cs, define an option ‘sc’ that calls a method safeComposedQuery(). Also, in QueryConstructor.cs, define method safeComposedQuery(). The new function should be an improvement over composedQuery() in two ways: firstly, it must call the stored database function safe_course(); and secondly, it must use separate parameter passing, including by calling query/3 as defined in PostgreSQL_client.cs.*
> 
The `safeComposedQuery` function in `QueryConstructor.cs` (defined in `Program.cs`'s `sc` option) performs a secure query by calling the `safe_course` function. Unlike `composedQuery()`, which concatenates strings directly (a vulnerable practice), `safeComposedQuery` leverages PostgreSQL's parameterized query feature in the `query/3` method to prevent injection.  

```Csharp
public void safeComposedQuery()
{
    string query = "SELECT * FROM safe_course(@p_id)";
    Console.Write("Please type id of a course: ");
    string? userValue = Console.ReadLine();
    Console.WriteLine($"Query to be executed: {query}");
    client.query(query, "p_id", userValue);
}
``` 

> NOTE: src\ComplexIT.SecuritySQL\QueryConstructor.cs

In this approach we use `@` sign, followed by the named parameter, to set the value. This separation of SQL and parameters ensures that any malicious inputs won’t alter the intended SQL logic.  

### Question 3. 
> *Define an SQL injection attack that works when option ‘c’ is selected, but fails when option ‘sc’ is selected. Provide a screenshot of the succesfull attack and a screenshot of the failed attack.*

The `composedQuery` method is vulnerable to SQL injection due to string concatenation, allowing injection attempts like `' AND 1=0 OR dept_name = 'Biology' --` to retrieve unintended data. However, this attack fails with `safeComposedQuery` because of parameterized querying, which treats the entire input as a single value, effectively neutralizing injection attempts.  

Test input: `' AND 1=0 OR dept_name = 'Biology' --`

#### S
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

#### SC
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

---
> NOTE : Questions 4 - 6 are about passwords.
---
### Question 4. 
> *Implement a check that the password provided by a new user at registration contains more than eight characters. Also check that the password does not contain the username. Obviously, the password ‘admindnc’ for username ‘admin’ will fail this test. Checks should be done in the definition of method passwordIsOK() in Authenticator.cs. (Perhaps you wonder why passwordIsOK() is defined as a virtual procedure. This allows for implementing modifications to the method in a subclass of class Authenticator. This helps me maintain program variants with and without modifications. You are free to hardcode you modifications directly into class Autenticator).*  

The changes done to passwordIsOkay function includes the checking of password length on regristration and that the password does not contain the username.  

```Csharp
public virtual bool passwordIsOK(string password, string username) 
{
    if (password.Length <= 8) return false;
    if (password.Contains(username)) return false;
    else  return true;
}
```

> NOTE: src\ComplexIT.SecurityPassword\Authenticator.cs  

Also the the passwordIsOkay call done in regristration was changed a bit for clarity.  

```Csharp
if (passwordIsOK(password, username) == false) 
{
    Console.WriteLine("Password is too weak");
    return false;
}
```

> NOTE: src\ComplexIT.SecurityPassword\Authenticator.cs  

Let us say a user would like to use the following credentials:  

```text
username: John
password: JohnDoe123
```

This would fail beacuse the username is in the provided password, but if the user were to split it up a bit, like:  

```text
username: John
password: J1ohnDoe23
```

Then the check would not see any issue with the provided password and it would return true, and allow the regristration.  

```text
Registration ..

Please type username: 
John // Input username

Please type password: 
J1ohnDoe23 // Input password

SQL to be inserted: insert into password values 

(
'John', // username
'A83E4C5E7EB755BE', // salt
'E1E559437076877D9CA38769020517FF36A07425384D4C668983BDC57EA50548' // hashed password
)

Registration succeeded
```

### Question 5. 
> *Implement iterative hashing. Hint: In the C# source file Hashing.cs, in the definition of method hashSHA256(), you may modify the second parameter in the call to function iteratedSha256(). What number of iterations appears to be reasonable on your computer?*  

The answer to the following question is going to dpeend a lot on the hardware on the PC, In the group we experienced quite different results, which is to be expected:  

```Csharp
private string hashSHA256(string password, string saltstring) 
{
    byte[] hashinput = Encoding.UTF8.GetBytes(saltstring + password); 
    byte[] hashoutput = iteratedSha256(hashinput, 2_000_000);
    return Convert.ToHexString(hashoutput);
}
```

>NOTE: src\ComplexIT.SecurityPassword\Hashing.cs  

Anything a bit above the 2 million mark starts to slow the process noticeably. We tried running it with 100 million iterations where it took around 4-5 seconds to compute the hash, which is not acceptable for most use cases. \
If one really wished to run more iterations over the password before storing it, they would need stronger hardware.  

---
> NOTE : Question 6 is about passwords and SQL injection.  
---
### Question 6. 
> *In Authenticator.cs, the method sqlSetUserRecord() defines a string that is an SQL command. The SQL command is then used in method register(). Is the method vulnerable to SQL injection?*  

Yes, In `Authenticator.cs`, the `sqlInsertUserRecord` method, it constructs SQL commands with string concatenation, essentially exposing it to SQL injection, if the inputs aren’t properly sanitized. Using parameterized statements with `NpgsqlParameter` would prevent this vulnerability.\  
When logging in the attacker can specify their username as follows, which would update the admin password with the attackers password:

```
' AND true; UPDATE password SET hashed_password = (SELECT hashed_password FROM password WHERE username = 'attacker'), salt = (SELECT salt FROM password WHERE username = 'attacker') WHERE username = 'admin'; --
```

This can also be done when registering a new user. This require the user to first register a user (the 'attacker') with a known password, this password hash can then be used next time to override the admin password with the attacker password.

```
dummy', 'salt', 'hash'); UPDATE password SET hashed_password = (SELECT hashed_password FROM password WHERE username = 'attacker'), salt = (SELECT salt FROM password WHERE username = 'attacker') WHERE username = 'admin'; --
```
