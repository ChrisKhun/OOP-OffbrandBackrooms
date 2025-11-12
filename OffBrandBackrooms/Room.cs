using System;
using System.Collections.Generic;
using System.Linq;

namespace OffBrandBackrooms
{
    public class Room
    {
        public string Name { get; set; }
        private Dictionary<string, Player> _players;
        private Dictionary<string, NPC> _npcs;
        private List<Item> _items;
        private Dictionary<string, Room> _connections;

        public Room() : this("NAMELESS") { }

        public Room(string name)
        {
            Name = name;
            _players = new Dictionary<string, Player>();
            _npcs = new Dictionary<string, NPC>();
            _items = new List<Item>();
            _connections = new Dictionary<string, Room>();
        }

        public Boolean AddPlayer(Player player)
        {
            if (!FindPlayer(player.Name))
            {
                _players[player.Name] = player;
                player.CurrentRoom = this;
                return true;
            }
            return false;
        }

        public Boolean RemovePlayer(Player player)
        {
            if (FindPlayer(player.Name))
            {
                _players.Remove(player.Name);
                return true;
            }
            return false;
        }

        public Boolean AddNPC(NPC npc)
        {
            if (!FindNPCS(npc.Name))
            {
                _npcs[npc.Name] = npc;
                return true;
            }
            return false;
        }

        public Boolean RemoveNPC(NPC npc)
        {
            if (FindNPCS(npc.Name))
            {
                _npcs.Remove(npc.Name);
                return true;
            }
            return false;
        }

        public Boolean AddItem(Item item)
        {
            _items.Add(item);
            return true;
        }

        public Boolean RemoveItem(Item item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                return true;
            }
            return false;
        }

        public string Description
        {
            get
            {
                string output = $"\nRoom: {Name} with {_players.Count} player{(_players.Count == 1 ? "" : "s")} and {_npcs.Count} entit{(_npcs.Count == 1 ? "y" : "ies")} present.\n";
                output += $"{"Connected Rooms:".PadRight(41)}{"Items:".PadRight(30)}{"Entity:".PadRight(20)}\n";

                int maxCount = Math.Max(Math.Max(_connections.Count, _items.Count), Math.Max(_npcs.Count, 1));

                for (int i = 0; i < maxCount; i++)
                {
                    string direction = i < _connections.Count ? _connections.Keys.ElementAt(i) : "";
                    string room = i < _connections.Count ? _connections.Values.ElementAt(i)?.Name ?? "" : "";
                    string item = i < _items.Count ? _items[i].Name : "";
                    string enemy = "";

                    if (i < _npcs.Count)
                    {
                        var npc = _npcs.Values.ElementAt(i);
                        enemy = npc.Name;
                        if (npc.Health <= 0)
                        {
                            enemy += " [DEAD]";
                        }
                    }

                    if (string.IsNullOrEmpty(direction) && string.IsNullOrEmpty(room) && string.IsNullOrEmpty(item) && string.IsNullOrEmpty(enemy))
                        continue;
                    string directionText = string.IsNullOrEmpty(direction) ? "" : (direction + ":");
                    output += $"{directionText.PadRight(16)}{room.PadRight(25)}{item.PadRight(30)}{enemy.PadRight(30)}\n";
                    }   
                return output;
            }
        }


        public Room? GetConnectedRoom(string direction)
        {
            _connections.TryGetValue(direction, out Room? room);
            return room;
        }

        public void ConnectRoom(string direction, Room room)
        {
            if (!_connections.ContainsKey(direction))
            {
                _connections[direction] = room;
                room.ConnectRoom(GetOppositeDirection(direction), this);
            }
        }

        private string GetOppositeDirection(string direction)
        {
            return direction switch
            {
                "north" => "south",
                "south" => "north",
                "east" => "west",
                "west" => "east",
                _ => throw new ArgumentException("Invalid direction")
            };
        }

        private Boolean FindPlayer(string name)
        {
            return _players.ContainsKey(name);
        }

        private Boolean FindNPCS(string name)
        {
            return _npcs.ContainsKey(name);
        }

        private Boolean FindItem(string name)
        {
            foreach (var item in _items)
            {
                if (item.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public Item? GetItem(string itemName)
        {
            foreach (var item in _items)
            {
                if (item.Name == itemName)
                {
                    return item;
                }
            }
            return null;
        }

        public NPC? GetNPC(string npcName)
        {
            foreach (var npc in _npcs.Values)
            {
                if (npc.Name == npcName)
                {
                    return npc;
                }
            }
            return null;
        }

        private string GetExits()
        {
            List<string> exits = new List<string>();
            foreach (var direction in _connections.Keys)
            {
                exits.Add(direction);
            }
            return string.Join(", ", exits);
        }
    }
}
