using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class Parser
    {
        private Dictionary<string, Command> _commands;

        // Default commands array, including basic commands to extend later
        private static Command[] defaultCommands =
        {
            // Additional commands can be added here
            new ListAllCommands(),
            new ListAllRooms(),
            new StartGame(),
            new CreatePlayer(),
            new MovePlayer(),
            new MoveBack(),
            new PlayerStats(),
            new CurrentRoomDescription(),
            new PickupItem(),
            new DropItem(),
            new CheckInventory(),
            new Equip(),
            new Useable(),
            new Unequip(),
            new Craft(),
            new Fight(),
            new Talk(),
            new ListStageState(),
            new Teleport()
        };

        // ----------------------- Constructor ---------------------------------------------
        public Parser()
        {
            _commands = new Dictionary<string, Command>();
            foreach (Command command in defaultCommands)
            {
                // Initialize the command dictionary with command names as keys
                _commands[command.Name] = command;
            }
        }

        // ----------------------- Parse Input Function ------------------------------------
        public Command? parseInput(string inputString)
        {
            Command? command = null;
            string[] words = inputString.Split(' '); // Split the input string by spaces

            // Try to find the command based on the first word in the input
            _commands.TryGetValue(words[0], out command);

            // If command is found, assign additional parameters
            if (command != null)
            {
                if (words.Length > 1)
                {
                    command.Parameter0 = words[1]; // First parameter
                    if (words.Length > 2)
                    {
                        command.Parameter1 = words[2]; // Second parameter
                        if (words.Length > 3)
                        {
                            command.Parameter2 = words[3]; // Third parameter
                        }
                    }
                }
            }

            return command; // Return the command or null if not found
        }
    }
}
