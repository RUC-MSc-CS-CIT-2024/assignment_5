# Assignment 5 - Security
## Complex IT Systems - Fall 2024
> NOTE : Questions 1-3 are about SQL Injection.

### Question 1. 
*Define a stored database function that returns a table of courses from the course table in the university database. You may name the stored function safe_course(). The function should have one input parameter of type VARCHAR(8). The function should return courses whose course id match the input. (There will be at most one such course, since the course id is a primary key of the table). Naturally, the function should leave out any course offered by the Biology department. Show the SQL code that defines the function.*

```sql
CREATE OR REPLACE FUNCTION "public"."safe_course"("course_id_in" varchar)
  RETURNS TABLE("course_id" varchar, "title" varchar, "dept_name" varchar) AS $BODY$
  BEGIN
    RETURN QUERY 
    SELECT c.course_id, c.title, c.dept_name 
    FROM course AS c 
    WHERE c.course_id = course_id_in 
    AND c.dept_name != 'Biology';
  END;
 $BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000
```

> NOTE: safe_course(course_id) function in postgres DB university

(Short Description of solution) ...

### Question 2. 
*In SQL-Injection-Frontend, in the switch statement of Program.cs, define an option ‘sc’ that calls a method safeComposedQuery(). Also, in QueryConstructor.cs, define method safeComposedQuery(). The new function should be an improvement over composedQuery() in two ways: firstly, it must call the stored database function safe_course(); and secondly, it must use separate parameter passing, including by calling query/3 as defined in PostgreSQL_client.cs.*

```Csharp
internal void safeComposedQuery()
{
    string sql = "SELECT * FROM safe_course($1);";
    Console.Write("Please type id of a course: ");
    string? user_defined = Console.ReadLine();
    client.query(sql, user_defined, null);
}
``` 

> NOTE: src\ComplexIT.SecuritySQL\QueryConstructor.cs

(Short Description of solution) ...

### Question 3. 
*Define an SQL injection attack that works when option ‘c’ is selected, but fails when option ‘sc’ is selected. Provide a screenshot of the succesfull attack and a screenshot of the failed attack.*

> NOTE : Questions 4 - 6 are about passwords.

### Question 5. 
*Implement a check that the password provided by a new user at registration contains more than eight characters. Also check that the password does not contain the username. Obviously, the password ‘admindnc’ for username ‘admin’ will fail this test. Checks should be done in the definition of method passwordIsOK() in Authenticator.cs. (Perhaps you wonder why passwordIsOK() is defined as a virtual procedure. This allows for implementing modifications to the method in a subclass of class Authenticator. This helps me maintain program variants with and without modifications. You are free to hardcode you modifications directly into class Autenticator).*

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

```
username: John
password: JohnDoe123
```

This would fail beacuse the username is in the provided password, but if the user were to split it up a bit, like:

```
username: John
password: J1ohnDoe23
```

Then the check would not see any issue with the provided password and it would return true, and allow the regristration.

```
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
*Implement iterative hashing. Hint: In the C# source file Hashing.cs, in the definition of method hashSHA256(), you may modify the second parameter in the call to function iteratedSha256(). What number of iterations appears to be reasonable on your computer?*

The answer to the following question is going to dpeend a lot on the hardware in the PC used to run. For my school laptop the limit were the following:

```Csharp
private string hashSHA256(string password, string saltstring) 
{
    byte[] hashinput = Encoding.UTF8.GetBytes(saltstring + password); 
    byte[] hashoutput = iteratedSha256(hashinput, 2_000_000);
    return Convert.ToHexString(hashoutput);
}
```

>NOTE: src\ComplexIT.SecurityPassword\Hashing.cs

Anything a bit above the 2 million mark starts to slow the process noticeably. I tried running it with 100 million iterations where it took around 4-5 seconds to compute the hash, which is not acceptable for most use cases. \
If one really wished to run more iterations over the password before storing it, they would need stronger hardware.

> NOTE : Question 6 is about passwords and SQL injection.

### Question 6. 
*In Authenticator.cs, the method sqlSetUserRecord() defines a string that is an SQL command. The SQL command is then used in method register(). Is the method vulnerable to SQL injection?*

Yes
