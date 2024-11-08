// Program.cs
// Entry point of the application SQL Injection Frontend

// Program.cs defines a simple 'looping' interface
// that asks the end user for input.
// In response to user input,
// the program calls an instance of class QueryConstructor

// Welcome message
Console.WriteLine("Welcome to SQL Injection Frontend\n");

QueryConstructor qConstructor = new QueryConstructor();
// QueryConstructor qConstructor = new QueryConstructorSolution();
// class QueryConstructorSolution contains assignment solutions
// and is not provided on moodle

// the user interface
string? s = "x";

do {
  Console.Write("Please select character + enter\n"
          + "'d' (dynamic query)\n"
          + "'c' (composed query)\n"
          + "'x' (exit)\n"
          + ">");
  s = Console.ReadLine();
  Console.WriteLine();
  switch (s) {
     case "d":
       qConstructor.dynamicQuery();
       break;
     case "c":
       qConstructor.composedQuery();
       break;
     case "x": 
       Console.WriteLine("exiting ..");
       break;
     default:
       Console.WriteLine("you typed " + "'" + s + "'" + " -- please use a suggested value");
       break;
   } // end switch
} while (s != "x");

