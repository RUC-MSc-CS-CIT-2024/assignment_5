// QueryConstructor.cs

public class QueryConstructor {

  public QueryConstructor() {
    client = new PostgreSQL_Client("uni", "postgres", "postgres");
    // retain university database
    // but change username and password
  }
 
  public PostgreSQL_Client client;

  // method definitions

  // dynamicQuery()
  // Get user-defined query, send query to db

  public void dynamicQuery() {
    // defining the query 
    Console.Write("Please type any SQL query: ");
    string? sql = Console.ReadLine();
  
    // printing query string to console
    Console.Write("Query to be executed: ");
    ConsoleColor currColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(sql+ "\n");
    Console.ForegroundColor = currColor;

    // executing query
    client.query(sql);
  } // end method dynamicQuery() 

  // composedQuery()
  // Get dynamic part from user,
  // then compose dynamic and static part,
  // and send query to db

  virtual public void composedQuery() {
    // defining the query
    string staticSQLbefore = "select * from course where course_id LIKE '%";
    Console.Write("Please type id of a course: ");
    string? user_defined = Console.ReadLine();
    string staticSQLafter = "%' and dept_name != 'Biology'";
    string sql = staticSQLbefore + user_defined + staticSQLafter;

    // printing query string to console
    Console.Write("Query to be executed: " + staticSQLbefore);
    
    ConsoleColor currColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write(user_defined);
    Console.ForegroundColor = currColor;
    Console.WriteLine(staticSQLafter+ "\n");

    // executing query
    client.query(sql);
  }

  public void safeComposedQuery() {
    string query = "SELECT * FROM safe_course(@p_id)";

    Console.Write("Please type id of a course: ");
    string? userValue = Console.ReadLine();

    Console.WriteLine($"Query to be executed: {query}");

    client.query(query, "p_id", userValue);
  }
}
