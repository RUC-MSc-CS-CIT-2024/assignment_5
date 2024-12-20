﻿// Program.cs

Console.WriteLine("Welcome to the password-based authenticator");
Console.WriteLine("");

Authenticator auth = new Authenticator();

Console.WriteLine("Registering user admin");
auth.register("a", "abc");


// the end-user interface
string? s = "x";

do {
  Console.Write("Please select character + enter\n"
          + "'r' (register)\n"
          + "'l' (login)\n"
          + "'x' (exit)\n"
          + ">");
  s = Console.ReadLine();
  Console.WriteLine();
  switch (s) {
     case "r":
       register();
       break;
     case "l":
       login();
       break;
     case "x": 
       Console.WriteLine("exiting ..");
       break;
     default:
       Console.WriteLine("you typed " + "'" + s + "'" + " -- use a suggested value");
       break;
   } // end switch
} while (s != "x");

// register() and login()

void register() {
  Console.WriteLine("Registration .. ");
  string username = getUserInput("Please type username:");
  string password = getUserInput("Please type password:");
  bool registered = auth.register(username, password);
  if (registered) Console.WriteLine("Registration succeeded");
  else Console.WriteLine("Registration failed");
}

void login() {
  Console.WriteLine("Logging in .. ");
  string username = getUserInput("Please type username:");
  string password = getUserInput("Please type password:");
  bool loggedin = auth.login(username, password);
  if (loggedin) Console.WriteLine("Login succeeded");
  else Console.WriteLine("Login failed");
}

// helper functions for exit(), register() and login()

string getUserInput(string s) {
  Console.WriteLine(s);
  return Console.ReadLine() ?? readLineError();
}

string readLineError() {
  return "Error: no string read by Console.ReadLine()";
}




   
