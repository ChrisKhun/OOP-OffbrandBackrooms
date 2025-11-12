using System;
using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CommandFunctions commandfunctions = new CommandFunctions();
            commandfunctions.Start();
            Parser parser = new Parser();

            while (commandfunctions.Working)
            {
                Console.WriteLine("Welcome To Offbrand Backrooms\n");
                Console.WriteLine("Main Menu");
                Console.WriteLine("1.) New Game");
                Console.WriteLine("2.) List All Commands");
                Console.WriteLine("3.) Exit\n");
                Console.Write    ("Enter a number to select an option: ");
                int option = Convert.ToInt32(Console.ReadLine());
                Console.Write("\n\n");

                switch (option)
                {
                    case 1:
                        Console.WriteLine("New Game");
                        Console.WriteLine("Starting New Game...\n");
                        // prompt player creation info
                        Console.Write("Enter Player Name: ");
                        string? playerName = Console.ReadLine();
                        Console.Write("Enter Player Gender: ");
                        string? playerGender = Console.ReadLine();
                        string[] newGameCommands = { $"CP {playerName} {playerGender}", "SG" }; 
                        goToCommandLine(newGameCommands);
                        break;
                    case 2:
                        Console.WriteLine("Listing all Commands:");
                        string[] listAllCommands = { "LAC" };
                        goToCommandLine(listAllCommands);
                        break;
                    case 3:
                        Console.WriteLine("\nThanks for playing!");
                        string[] exitWithSave = { "exit" };
                        goToCommandLine(exitWithSave);
                        break;
                }
            }


            void goToCommandLine(string[] commandName)
            {
                foreach (string textCommand in commandName)
                {
                    Command? command = parser.parseInput(textCommand);
                    if (command != null)
                    {
                        command.Execute(commandfunctions);
                    }
                }

                Boolean working = true;
                while (working == true)
                {
                    Console.Write("\nEnter a command (or 'LAC' to ListAllCommands): ");
                    string? inputString = Console.ReadLine();

                    if (inputString == null)
                    {
                        Console.WriteLine("Input cannot be null. Please enter a valid command.");
                        continue;
                    }

                    Command? command = parser.parseInput(inputString);

                    if (inputString == "exit")
                    {
                        working = false;
                    }

                    if (command != null)
                    {
                        command.Execute(commandfunctions);
                    }
                }
            }
        }
    }
}
