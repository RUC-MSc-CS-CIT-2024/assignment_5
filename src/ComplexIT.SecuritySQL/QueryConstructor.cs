public class QueryConstructor {
  
  private readonly PostgreSQL_Client _client;

  public QueryConstructor() {
    _client = new PostgreSQL_Client("university", "postgres", "postgres", 5433);
  }
  
  public void dynamicQuery(string? sql) {
    PrintQuery(sql);
    _client.query(sql);
  }

  public virtual void composedQuery(string? id) {
    string staticSqlBefore = "select * from course where course_id = '";
    string staticSqlAfter = "' and dept_name != 'Biology'";
    string sql = staticSqlBefore + id + staticSqlAfter;
    
    PrintQuery(sql);
    _client.query(sql);
  }

  public virtual void safeComposedQuery(string? id)
  {
    string sql = "select * from safe_course(@id)";
    PrintQuery(sql);
    _client.query(sql,"@id",id);
  }
  
  private void PrintQuery(string sql)
  {
    Console.Write("Query to be executed: ");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(sql+ "\n");
    Console.ForegroundColor = ConsoleColor.White;
  }
}
