using System;
using System.Collections.Generic;

namespace OffBrandBackrooms
{
    public class Stage
    {
        public string Name { get; set; }
        public string StageState { get; private set; }
        private Dictionary<string, Room> _rooms;

        public Stage() : this("NAMELESS", "STATELESS") { }


        public Stage(string name, string stagestate)
        {
            Name = name;
            StageState = stagestate;
            _rooms = new Dictionary<string, Room>();
        }

        // Method to update the stage state
        public void UpdateStageState(string newState)
        {
            if (string.IsNullOrEmpty(newState))
            {
                Console.WriteLine("Invalid state provided. The state cannot be empty or null.");
                return;
            }
            StageState = newState;
            Console.WriteLine($"Stage '{Name}' state updated to: {StageState}");
        }

        // Add a room to the stage
        public Boolean AddRoom(Room room)
        {
            if (!_rooms.ContainsKey(room.Name))
            {
                _rooms[room.Name] = room;
                return true;
            }
            return false;
        }

        // Get a specific room by name
        public Room? GetRoom(string roomName)
        {
            _rooms.TryGetValue(roomName, out Room? room);
            return room;
        }

        // Get a description of all rooms
        public string Description
        {
            get
            {
                string output = $"\nStage: {Name} (State: {StageState}) contains the following rooms:";
                foreach (var room in _rooms.Values)
                {
                    output += $"\n\t- {room.Name}";
                }
                return _rooms.Count > 0 ? output : $"\nStage: {Name} (State: {StageState}) has no rooms.";
            }
        }

        // Get all room names
        public List<string> GetRoomNames()
        {
            return new List<string>(_rooms.Keys);
        }

        // Map and link rooms into a grid
        public bool MapStage(int rows, int cols)
        {
            if (_rooms.Count < rows * cols)
            {
                Console.WriteLine("Not enough rooms to fill the grid.");
                return false;
            }

            // Create a 2D grid for rooms
            Room[,] roomGrid = new Room[rows, cols];
            int roomIndex = 0;
            List<Room> roomList = new List<Room>(_rooms.Values);

            // fill the grid with rooms
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    roomGrid[row, col] = roomList[roomIndex];
                    roomIndex++;
                }
            }

            // Link the rooms 
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Room currentRoom = roomGrid[row, col];

                    if (row > 0) 
                    {
                        currentRoom.ConnectRoom("north", roomGrid[row - 1, col]);
                    }
                    if (row < rows - 1) 
                    {
                        currentRoom.ConnectRoom("south", roomGrid[row + 1, col]);
                    }
                    if (col > 0) 
                    {
                        currentRoom.ConnectRoom("west", roomGrid[row, col - 1]);
                    }
                    if (col < cols - 1) 
                    {
                        currentRoom.ConnectRoom("east", roomGrid[row, col + 1]);
                    }
                }
            } 
            return true;
        }
    }
}
