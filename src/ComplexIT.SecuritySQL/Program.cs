
Console.WriteLine("Welcome to SQL Injection Frontend\n");
QueryConstructor qConstructor = new QueryConstructor();
string? s;

do {
  Console.Write("Please select character + enter\n"
          + "'d' (dynamic query)\n"
          + "'c' (composed query)\n"
          + "'x' (exit)\n"
          + "'sc' (safe composed query)\n"
          + ">");
  s = Console.ReadLine();
  Console.WriteLine();
  switch (s) {
     case "d":
       Console.Write("Please type any SQL query: ");
       string? sql = Console.ReadLine();
       qConstructor.dynamicQuery(sql);
       break;
     case "c":
       Console.Write("Please type id of a course: ");
       string? cid = Console.ReadLine();
       qConstructor.composedQuery(cid);
       break;
     case "sc":
       Console.Write("Please type id of a course: ");
       string? csid = Console.ReadLine();
       qConstructor.safeComposedQuery(csid);
       break;
     case "x": 
       Console.WriteLine("exiting ..");
       break;
     default:
       Console.WriteLine("you typed " + "'" + s + "'" + " -- please use a suggested value");
       break;
   } // end switch
} while (s != "x");

