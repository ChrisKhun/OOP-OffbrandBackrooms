using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffBrandBackrooms;
using System.Collections.Generic;



namespace OffBrandBackrooms
{
    public class CommandFunctions
    {
        public string Name { get; set; }
        private Dictionary<string, Player> _player;
        private Dictionary<string, NPC> _npcs;
        private Dictionary<string, Room> _rooms;
        private Dictionary<string, Stage> _stages;
        private Dictionary<string, Item> _items;
        private Dictionary<string, Inventory> _inventories;
        private Stack<Command> _commandLog;
        private Boolean _working;
        public Boolean Working { get { return _working; } private set { _working = value; } }
        public CommandFunctions() : this("NO NAME") { }

        private Player? currentPlayer;
        public Player? CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }

        Stack<string> MapStack = new Stack<string>();
        List<string> ExcludeRooms = new List<string> { "DefaultBoss", "DefaultShop", "AbandonedUtilityHallsBoss", "AbandonedUtilityHallsShop",
        "InfiniteApartmentsBoss", "InfiniteApartmentsShop", "PoolBoss", "PoolShop", "IkeaBoss", "IkeaShop", "ArcadeBoss", "ArcadeShop",
        "SnackRoomBoss", "SnackRoomShop", "CarParkBoss", "CarParkShop" };

        // ----------------------- Constructor --------------------------------------------
        public CommandFunctions(string name)
        {
            Name = name;
            _player = new Dictionary<string, Player>();
            _npcs = new Dictionary<string, NPC>();
            _rooms = new Dictionary<string, Room>();
            _stages = new Dictionary<string,Stage>();
            _items = new Dictionary<string, Item>();
            _inventories = new Dictionary<string, Inventory>();
            Working = false;
            _commandLog = new Stack<Command>();
        }

        // ----------------------- Execute Command ----------------------------------------
        public Boolean Execute(Command command)
        {
            _commandLog.Push(command);
            return command.Execute(this);
        }

        // ----------------------- Start Function -----------------------------------------
        public Boolean Start()
        {
            Boolean result = false;
            Working = true;
            result = false;
            return result;
        }

        // ----------------------- Player Functions ---------------------------------
        public Boolean CreatePlayer(string name, string gender)
        {
            Boolean result = false;
            if (!_player.ContainsKey(name + " " + gender)) 
            {
                _player[name + " " + gender] = new Player(name, gender, 100, 0, 0, 25.0f, 3);
                SetCurrentPlayer(name);
                CreateInventory(name);
                result = true;
                return result;
            }
            return result;
        }

        public bool PlayerStats()
        {
            Player? player = currentPlayer;

            if (player == null)
            {
                Console.WriteLine("No player data available.");
                return false;
            }

            Console.WriteLine($"Health: {player.Health}");
            Console.WriteLine($"Shield: {player.Shield}");
            Console.WriteLine($"Scrap: {player.Scrap}");
            Console.WriteLine($"Attack Damage: {player.AttackDamage}");
            Console.WriteLine($"Weight: {player.CurrentWeight}");

            if (player.CurrentWeapon != null)
            {
                Console.WriteLine($"Current Weapon: {player.CurrentWeapon.Name}");
            }

            return true;
        }


        public Boolean SetCurrentPlayer(string playerName)
        {
            Boolean result = false;
            Player? player = FindPlayer(playerName);
            if (player != null)
            {
                currentPlayer = player;
                result = true;
                return result;
            }
            Console.WriteLine($"{Name} not found");
            return result;
        }

        private Player? FindPlayer(string name)
        {
            foreach (var player in _player.Values)
            {
                if (player.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return player;
                }
            }
            Console.WriteLine($"Player '{name}' not found.");
            return null;
        }

        public Boolean MovePlayer(string? direction)
        {
            Boolean result = false;
            Player? player = currentPlayer;
            Room? currentRoom = player?.CurrentRoom;
            Room? targetRoom = null;
            if (player != null)
            {
                switch (direction?.ToLower())
                {
                    case "north":
                        targetRoom = currentRoom?.GetConnectedRoom("north");
                        break;
                    case "west":
                        targetRoom = currentRoom?.GetConnectedRoom("west");
                        break;
                    case "south":
                        targetRoom = currentRoom?.GetConnectedRoom("south");
                        break;
                    case "east":
                        targetRoom = currentRoom?.GetConnectedRoom("east");
                        break;
                    default:
                        Console.WriteLine("Invalid direction. Please choose north, south, east, or west.");
                        return result;
                }

                if (targetRoom != null)
                {
                    if (targetRoom.Name.EndsWith("Boss"))
                    {
                        // put logic to check if inventory has key matching with Boss Room name and deny if key not found
                    }

                    if (currentRoom != null)
                    {
                        MapStack.Push(currentRoom.Name);
                        RemovePlayerFromRoom(currentRoom.Name);
                        AddPlayerToRoom(targetRoom.Name);
                    }
                    
                    if (!MapStack.Contains(targetRoom.Name) && !ExcludeRooms.Contains(targetRoom.Name))     // if room has been visited then no items will spawn
                    {
                        ItemCreation();
                    }

                    Console.WriteLine($"Player '{player?.Name}' moved to {targetRoom.Name}.");
                    Console.WriteLine(targetRoom.Description);
                    Console.WriteLine("\nHint: Use move <direction>");
                    result = true;
                    return result;
                }
                else
                {
                    Console.WriteLine("There is no room in that direction.");
                    return result;
                }
            }
            return result;
        }

        public Boolean MoveBack()
        {
            Boolean result = false;
            Player? player = currentPlayer;
            if (player != null)
            {
                if (MapStack.Count > 0)
                {
                    Room? room = player.CurrentRoom;
                    room?.RemovePlayer(player);
                    Room? previousRoom = FindRoom(MapStack.Pop());  // Pop the last room from the stack
                    previousRoom?.AddPlayer(player);
                    Console.WriteLine(previousRoom?.Description);
                }
                else
                {
                    Console.WriteLine("Sorry. No Room To Go Back To!");
                }
            }
            return result;
        }

        public Boolean MoveToCheckpoint()
        {
            Boolean result = false;
            Player? player = currentPlayer;
            Room? room = player?.CurrentRoom;
            Room? checkpoint = player?.Checkpoint;
            if (player != null)
            {
                room?.RemovePlayer(player);
                checkpoint?.AddPlayer(player);
                Console.WriteLine(checkpoint?.Description);
            }
            return result;
        }

        public Boolean SetCheckpoint()
        {
            Boolean result = false;
            Player? player = currentPlayer;
            Room? room = player?.CurrentRoom;
            if (player != null)
            {
                player.Checkpoint = room;
            }
            return result;
        }

        public Boolean PlacePlayerInRndLocation(string stageName)  // places player in random location within declared room
        {
            Boolean result = false;
            Player? player = currentPlayer;
            RoomInfo roominfo = new RoomInfo();
            if (player != null)
            {
                List<string> roomGroup;
                switch (stageName)
                {
                    case "Stage1":
                        roomGroup = roominfo.defaultRoom.Take(14).ToList();
                        break;
                    case "Stage2":
                        roomGroup = roominfo.abandonedUtilityHalls.Take(14).ToList();
                        break;
                    case "Stage3":
                        roomGroup = roominfo.infiniteApartments.Take(14).ToList();
                        break;
                    case "Stage4":
                        roomGroup = roominfo.poolRoom.Take(14).ToList();
                        break;
                    case "Stage5":
                        roomGroup = roominfo.ikeaRoom.Take(14).ToList();
                        break;
                    case "Stage6":
                        roomGroup = roominfo.arcadeRoom.Take(14).ToList();
                        break;
                    case "Stage7":
                        roomGroup = roominfo.snackRoom.Take(14).ToList();
                        break;
                    case "Stage8":
                        roomGroup = roominfo.carPark.Take(14).ToList();
                        break;
                    default:
                        Console.WriteLine("Invalid Stage, Try Again!");
                        return false;
                }
                Shuffle(roomGroup);
                string rndRoom = roomGroup[0];
                Room? room = FindRoom(rndRoom);
                if (room != null)
                {
                    room.AddPlayer(player);
                    result = true;
                    return result;
                }
                
            }
            Console.WriteLine("player not found.");
            return result;
        }

        public Boolean Teleport()
        {
            Boolean result = false;
            Player? player = currentPlayer;
            Room? room = player?.CurrentRoom;
            RoomInfo roominfo = new RoomInfo();

            if (room != null)
            if (room.Name.EndsWith("BossRoom"))
            {
                // create a list of teleportable stages and it teleports player to new stage

                int maxLength = Math.Min(roominfo.bosses.Count, roominfo.bossroomNames.Count);

                for (int i = 0; i < maxLength; i++)
                {
                    string bossName = roominfo.bosses[i];
                    string roomName = roominfo.bossroomNames[i];
                    NPC? npc = FindNPCInRoom(bossName, roomName);         
                    string? npcStage = npc?.CurrentStage;
                    if (npcStage != null) 
                    {
                        Stage? stage = FindStage(npcStage);
                        if (stage?.StageState == "UNLOCKED")
                        {
                            Console.WriteLine($"{i+1}. {roomName} [{stage?.StageState}]");
                        }                        
                    }                    
                }
                Console.Write("\nEnter the stage number you want to teleport to (or 0 to cancel): ");
                        int option = Convert.ToInt32(Console.ReadLine());
                        switch(option)
                        {
                            case 1:
                                PlacePlayerInRndLocation("Stage1");
                                CurrentRoomDescription();
                                Console.WriteLine("Now in Stage 1");
                                break;
                            case 2:
                                PlacePlayerInRndLocation("Stage2");
                                CurrentRoomDescription();
                                Console.WriteLine("Now in Stage 2");
                                break;
                            case 3:
                                PlacePlayerInRndLocation("Stage3");
                                CurrentRoomDescription();
                                Console.WriteLine("Now in Stage 3");
                                break;
                            case 4:
                                PlacePlayerInRndLocation("Stage4");
                                CurrentRoomDescription();
                                Console.WriteLine("Now in Stage 4");
                                break;
                            case 5:
                                PlacePlayerInRndLocation("Stage5");
                                CurrentRoomDescription();
                                Console.WriteLine("Now in Stage 5");
                                break;
                            case 6:
                                PlacePlayerInRndLocation("Stage6");
                                CurrentRoomDescription();
                                Console.WriteLine("Now in Stage 6");
                                break;
                            case 7:
                                PlacePlayerInRndLocation("Stage7");
                                CurrentRoomDescription();
                                Console.WriteLine("Now in Stage 7");
                                break;
                            case 8:
                                PlacePlayerInRndLocation("Stage8");
                                CurrentRoomDescription();
                                Console.WriteLine("Now in Stage 8");
                                break;
                            case 0:
                                Console.WriteLine("Going Back...");
                                CurrentRoomDescription();
                                return result;
                            default:
                                Console.WriteLine("Invalid stage! Please select a stage between 1 and 8.");
                                break;
                        }
                result = true;
                return result;
            }
            Console.WriteLine("Sorry, you aren't in a boss room so you cannot teleport to other stages!");
            return result;
        }

        // ----------------------- Inventory Functions ----------------------------------
        
        public Boolean AddItemToInventory(string itemName, string? itemNameExt, string? itemNameExt2)
        {
            Boolean result = false;
            Player? player = currentPlayer;

            itemName = $"{itemName} {itemNameExt} {itemNameExt2}".Trim();
            Console.WriteLine($"Attempting to pick up: '{itemName}'");

            if (player != null)
            {
                Inventory? inventory = FindInventory(player.Name);
                Item? item = FindItemInCurrentRoom(itemName);
                if (item != null)
                {
                    Room? currentRoom = player.CurrentRoom;
                    if (currentRoom != null)
                    {
                        inventory?.AddItem(item);
                        player.CurrentWeight -= item.Weight;
                        if (player.CurrentWeight < 0)
                        {
                            inventory?.RemoveItem(item);
                            currentRoom.AddItem(item);
                            Console.WriteLine("You have exceeded the maximum weight. Drop some items!");
                            return result;
                        }
                        if (player.CurrentWeight > 0)
                        {
                            currentRoom.RemoveItem(item);
                            Console.WriteLine($"You picked up {itemName}");
                            result = true;
                            return result;
                        }
                    }
                }
                Console.WriteLine("Item not found");
                return result;
            }
            Console.WriteLine("Player not found");
            return result;
        }


        public Boolean DropItemFromInventory(string itemName, string? itemNameExt, string? itemNameExt2)
        {
            Boolean result = false;
            Player? player = currentPlayer;

            itemName = $"{itemName} {itemNameExt} {itemNameExt2}".Trim();
            Console.WriteLine($"Attempting to drop: '{itemName}'");

            if (player != null)
            {
                Inventory? inventory = FindInventory(player.Name);
                Item? item = FindItemInInventory(itemName);
                if (item != null)
                {
                    Room? currentRoom = player.CurrentRoom;
                    inventory?.RemoveItem(item);
                    currentRoom?.AddItem(item);
                    player.CurrentWeight += item.Weight;
                    Console.WriteLine($"You dropped {itemName}");
                    result = true;
                    return result;
                    }
                Console.WriteLine("Item not found");
                return result;
            }
            Console.WriteLine("Player not found");
            return result;
        }


        public Boolean CheckInventory()
        {
            Boolean result = false;
            Player? player = currentPlayer;
            if (player != null)
            {
                Inventory? inventory = FindInventory(player.Name);
                if (inventory != null)
                {
                    Console.WriteLine(inventory.Description);
                    result = true;
                    return result;
                }
                Console.WriteLine($"{inventory} not found.");
                return result;
            }
            Console.WriteLine($"{player} not found."); 
            return result;
        }

        private Inventory? FindInventory(string inventoryName)
        {
            Inventory? inventory = null;
            _inventories.TryGetValue(inventoryName, out inventory);
            return inventory;
        }

        public Boolean CreateInventory(string name)
        {
            Boolean result = false;
            if (!_inventories.ContainsKey(name))
            {
                _inventories[name] = new Inventory(name);
                result = true;
                return result;
            }
            return result;
        }

        // ----------------------- Item Functions -------------------------------------

        public Boolean ItemCreation() // MOVE ALL ITEM INFO INTO ANOTHER FILE TO DECLUTTER
        {
            Boolean result = false;

            // [ItemType, ItemName, Weight, DamageAmount, SellValue, BuyValue, Useable, Equipable, Craftable, Rarity, Description]
            // for item type
            // Weapon = 0
            // Tools = 1
            // Armor = 2
            // Inventory Increaser = 3
            // Consumable = 4
            // Miscellaneous = 5   CANNOT BE EQUIPPED
            // Weapons
            // [ItemType, ItemName, Weight, DamageAmount, SellValue, BuyValue, Useable, Equipable, Craftable, Rarity, Description]
            Item SwordOfNull =      new Item(itemtype: 0, name: "Sword of Null", weight: 3, sellValue: 1250, 
                                            buyValue: null, useable: false, equipable: true, 
                                            rarity: "Legendary", description: "description", damageamount: 60);
            Item SharpenedKatana =  new Item(itemtype: 0, name: "Sharpened Katana", weight: 2.5f, sellValue: 400, 
                                            buyValue: 650, useable: false, equipable: true, 
                                            rarity: "Epic", description: "description", damageamount: 40);
            Item RustyPistol =      new Item(itemtype: 0, name: "Rusty Pistol", weight: 2, sellValue: 400, 
                                            buyValue: 600, useable: false, equipable: true, 
                                            rarity: "Epic", description: "description", damageamount: 30);
            Item MolotovCocktail =  new Item(itemtype: 0, name: "Molotov Cocktail", weight: 2, sellValue: 250, 
                                            buyValue: 400, useable: true, equipable: false, 
                                            rarity: "Epic", description: "description", damageamount: 25);
            Item Crossbow =         new Item(itemtype: 0, name: "Crossbow", weight: 6, sellValue: 350, 
                                            buyValue: 500, useable: false, equipable: true, 
                                            rarity: "Rare", description: "description", damageamount: 30);
            Item BarbedBaseballBat =new Item(itemtype: 0, name: "Barbed Baseball Bat", weight: 2, sellValue: 300, 
                                            buyValue: 450, useable: false, equipable: true, 
                                            rarity: "Rare", description: "description", damageamount: 18);
            Item FlareGun =         new Item(itemtype: 0, name: "Flare Gun", weight: 1, sellValue: 125, 
                                            buyValue: 200, useable: true, equipable: false, 
                                            rarity: "Rare", description: "description", damageamount: 0);
            Item Taser =            new Item(itemtype: 0, name: "Taser", weight: 0.15f, sellValue: 80, 
                                            buyValue: 120, useable: true, equipable: false, 
                                            rarity: "Rare", description: "description", damageamount: 0);
            Item RustySword =       new Item(itemtype: 0, name: "Rusty Sword", weight: 4, sellValue: 150, 
                                            buyValue: 250, useable: false, equipable: true, 
                                            rarity: "Uncommon", description: "description", damageamount: 20);
            Item RustedMachete =    new Item(itemtype: 0, name: "Rusted Machete", weight: 5, sellValue: 125, 
                                            buyValue: 200, useable: false, equipable: true, 
                                            rarity: "Uncommon", description: "description", damageamount: 18);
            Item PoolStick =        new Item(itemtype: 0, name: "Pool Stick", weight: 2, sellValue: 80, 
                                            buyValue: 120, useable: false, equipable: true, 
                                            rarity: "Common", description: "description", damageamount: 12);
            Item WoodenBoard =      new Item(itemtype: 0, name: "Wooden Board", weight: 1.5f, sellValue: 30, 
                                            buyValue: 50, useable: false, equipable: true, 
                                            rarity: "Common", description: "description", damageamount: 10);
            Item RustyPipe =        new Item(itemtype: 0, name: "Rusty Pipe", weight: 1, damageamount: 8, sellValue: 15, 
                                            buyValue: 30, useable: false, equipable: true, 
                                            rarity: "Common", description: "description");
            Item KitchenKnife =     new Item(itemtype: 0, name: "Kitchen Knife", weight: 1, sellValue: 10, 
                                            buyValue: 20, useable: false, equipable: true, 
                                            rarity: "Common", description: "description", damageamount: 6);
            Item RustyShovel =      new Item(itemtype: 0, name: "Rusty Shovel", weight: 3, sellValue: 5, 
                                            buyValue: 10, useable: false, equipable: true, 
                                            rarity: "Common", description: "description", damageamount: 4);
            Item BrokenBottle =     new Item(itemtype: 0, name: "Broken Bottle", weight: 0.15f, sellValue: 3, 
                                            buyValue: 5, useable: false, equipable: true, 
                                            rarity: "Common", description: "description", damageamount: 3);
            Item BagOfGlassShards = new Item(itemtype: 0, name: "Bag of Glass Shards", weight: 0.15f, sellValue: 3, 
                                            buyValue: 7, useable: true, equipable: false, 
                                            rarity: "Common", description: "description", damageamount: 0);

            // Tools
            Item StrangeCharm =    new Item(itemtype: 1, name: "Strange Charm", weight: 1, sellValue: 1000, 
                                            buyValue: null, useable: true, equipable: false, 
                                            rarity: "Legendary", description: "Description");
            Item RustyCrowbar =    new Item(itemtype: 1, name: "Rusty Crowbar", weight: 2, sellValue: 125, 
                                            buyValue: 200, useable: true, equipable: false, 
                                            rarity: "Rare", description: "Description");
            Item Flashlight =      new Item(itemtype: 1, name: "Flashlight", weight: 0.5f, sellValue: 100, 
                                            buyValue: 140, useable: true, equipable: false, 
                                            rarity: "Rare", description: "Description");
            Item Lockpicks =       new Item(itemtype: 1, name: "Lockpicks", weight: 0.25f, sellValue: 15, 
                                            buyValue: 25, useable: true, equipable: false, 
                                            rarity: "Uncommon", description: "Description");
            Item Hammer =          new Item(itemtype: 1, name: "Hammer", weight: 3, sellValue: 75, 
                                            buyValue: 150, useable: true, equipable: false, 
                                            rarity: "Uncommon", description: "Description");
            Item BobbyPinBox =     new Item(itemtype: 1, name: "Bobby Pin Box", weight: 0.5f, sellValue: 50, 
                                            buyValue: 75, useable: true, equipable: false, 
                                            rarity: "Common", description: "Description");
            Item DamagedCrowbar =  new Item(itemtype: 1, name: "Damaged Crowbar", weight: 2, sellValue: 25, 
                                            buyValue: 40, useable: true, equipable: false, 
                                            rarity: "Common", description: "Description");

            // Armor
            Item CombatArmor =     new Item(itemtype: 2, name: "Combat Armor", weight: 5f, sellValue: 700, 
                                            buyValue: 800, useable: false, equipable: true, 
                                            rarity: "Legendary", description: "Shield up by 100+", shieldincrease: 100);
            Item LeatherJacket =   new Item(itemtype: 2, name: "Leather Jacket", weight: 3.5f, sellValue: 500, 
                                            buyValue: 650, useable: false, equipable: true, 
                                            rarity: "Epic", description: "Shield up by 75+", shieldincrease: 75);
            Item TacticalVest =    new Item(itemtype: 2, name: "Tactical Vest", weight: 4f, sellValue: 600, 
                                            buyValue: 750, useable: false, equipable: true, 
                                            rarity: "Epic", description: "Shield up by 85+", shieldincrease: 85);
            Item ReinforcedArmor = new Item(itemtype: 2, name: "Reinforced Armor", weight: 4.5f, sellValue: 400, 
                                            buyValue: 550, useable: false, equipable: true, 
                                            rarity: "Rare", description: "Shield up by 60+", shieldincrease: 60);
            Item BulletproofVest = new Item(itemtype: 2, name: "Bulletproof Vest", weight: 3.75f, sellValue: 450, 
                                            buyValue: 600, useable: false, equipable: true, 
                                            rarity: "Rare", description: "Shield up by 50+", shieldincrease: 50);
            Item KevlarJacket =    new Item(itemtype: 2, name: "Kevlar Jacket", weight: 2.5f, sellValue: 180, 
                                            buyValue: 250, useable: false, equipable: true, 
                                            rarity: "Uncommon", description: "Shield up by 25+", shieldincrease: 25);
            Item TacticalShirt =   new Item(itemtype: 2, name: "Tactical Shirt", weight: 2f, sellValue: 150, 
                                            buyValue: 220, useable: false, equipable: true, 
                                            rarity: "Uncommon", description: "Shield up by 20+", shieldincrease: 20);
            Item SportsArmor =     new Item(itemtype: 2, name: "Sports Armor", weight: 1.5f, sellValue: 60, 
                                            buyValue: 90, useable: false, equipable: true, 
                                            rarity: "Common", description: "Shield up by 10+", shieldincrease: 10);
            Item PaddedShirt =     new Item(itemtype: 2, name: "Padded Shirt", weight: 0.5f, sellValue: 30, 
                                            buyValue: 50, useable: false, equipable: true, 
                                            rarity: "Common", description: "Shield up by 5+", shieldincrease: 5);
            Item DirtyTShirt =     new Item(itemtype: 2, name: "Dirty T-Shirt", weight: 0.25f, sellValue: 15, 
                                            buyValue: 30, useable: false, equipable: true, 
                                            rarity: "Common", description: "No shield protection", shieldincrease: 0);

            // Backpacks
            Item TitanStoragePack = new Item(itemtype: 3, name: "Titan Storage Pack", weight: 6f, sellValue: 1500, 
                                                buyValue: 2000, useable: false, equipable: true, 
                                                rarity: "Legendary", description: "Increases carry weight by 70lbs", invincrease: 70);
            Item GrandExplorersBackpack = new Item(itemtype: 3, name: "Grand Explorer's Backpack", weight: 5f, sellValue: 1200, 
                                                buyValue: 1600, useable: false, equipable: true, 
                                                rarity: "Legendary", description: "Increases carry weight by 50lbs", invincrease: 50);
            Item MegaStorageBag = new Item(itemtype: 3, name: "Mega Storage Bag", weight: 4.5f, sellValue: 1000, 
                                                buyValue: 1300, useable: false, equipable: true, 
                                                rarity: "Legendary", description: "Increases carry weight by 40lbs", invincrease: 40);
            Item ExplorersBackpack = new Item(itemtype: 3, name: "Explorer's Backpack", weight: 3f, sellValue: 800, 
                                                buyValue: 1000, useable: false, equipable: true, 
                                                rarity: "Epic", description: "Increases carry weight by 30lbs", invincrease: 30);
            Item AdventureBag = new Item(itemtype: 3, name: "Adventure Bag", weight: 2f, sellValue: 400, 
                                                buyValue: 600, useable: false, equipable: true, 
                                                rarity: "Rare", description: "Increases carry weight by 20lbs", invincrease: 20);
            Item OutdoorRucksack = new Item(itemtype: 3, name: "Outdoor Rucksack", weight: 2.25f, sellValue: 450, 
                                                buyValue: 700, useable: false, equipable: true, 
                                                rarity: "Rare", description: "Increases carry weight by 18lbs", invincrease: 18);
            Item BasicBackpack = new Item(itemtype: 3, name: "Basic Backpack", weight: 1.25f, sellValue: 100, 
                                                buyValue: 150, useable: false, equipable: true, 
                                                rarity: "Uncommon", description: "Increases carry weight by 8lbs", invincrease: 8);
            Item CargoShorts = new Item(itemtype: 3, name: "Cargo Shorts", weight: 0.75f, sellValue: 50, 
                                                buyValue: 85, useable: false, equipable: true, 
                                                rarity: "Common", description: "Increases carry weight by 6lbs", invincrease: 6);
            Item CanvasBag = new Item(itemtype: 3, name: "Canvas Bag", weight: 0.75f, sellValue: 30, 
                                                buyValue: 50, useable: false, equipable: true, 
                                                rarity: "Common", description: "Increases carry weight by 5lbs", invincrease: 5);
            Item FannyPack = new Item(itemtype: 3, name: "Fanny Pack", weight: 0.5f, sellValue: 20, 
                                                buyValue: 35, useable: false, equipable: true, 
                                                rarity: "Common", description: "Increases carry weight by 3lbs", invincrease: 3);

            // Consumable
            Item Burger =         new Item(itemtype: 4, name: "Burger", weight: 0.25f, sellValue: 1000, 
                                            buyValue: 1500, useable: true, equipable: false, 
                                            rarity: "Legendary", description: "Restores 100 health", healthincrease: 100);
            Item Antidote =       new Item(itemtype: 4, name: "Antidote", weight: 0.2f, sellValue: 400, 
                                            buyValue: 600, useable: true, equipable: false, 
                                            rarity: "Epic", description: "Restores 80 health", healthincrease: 80);
            Item EnergyDrink =    new Item(itemtype: 4, name: "Energy Drink", weight: 0.2f, sellValue: 250, 
                                            buyValue: 350, useable: true, equipable: false, 
                                            rarity: "Rare", description: "Restores 60 health", healthincrease: 60);
            Item Bandages =       new Item(itemtype: 4, name: "Bandages", weight: 0.75f, sellValue: 100, 
                                            buyValue: 150, useable: true, equipable: false, 
                                            rarity: "Uncommon", description: "Restores 40 health", healthincrease: 40);
            Item Painkillers =    new Item(itemtype: 4, name: "Painkillers", weight: 0.1f, sellValue: 75, 
                                            buyValue: 125, useable: true, equipable: false, 
                                            rarity: "Uncommon", description: "Restores 30 health", healthincrease: 30);
            Item CannedSoup =     new Item(itemtype: 4, name: "Canned Soup", weight: 0.25f, sellValue: 50, 
                                            buyValue: 100, useable: true, equipable: false, 
                                            rarity: "Common", description: "Restores 20 health", healthincrease: 20);
            Item WaterBottle =    new Item(itemtype: 4, name: "Water Bottle", weight: 0.05f, sellValue: 25, 
                                            buyValue: 50, useable: true, equipable: false, 
                                            rarity: "Common", description: "Restores 15 health", healthincrease: 15);
            Item IkeaMeatball =   new Item(itemtype: 4, name: "Ikea Meatball", weight: 0.25f, sellValue: 30, 
                                            buyValue: 60, useable: true, equipable: false, 
                                            rarity: "Common", description: "Restores 25 health", healthincrease: 25);
            Item Gum =            new Item(itemtype: 4, name: "Gum", weight: 0.05f, sellValue: 10, 
                                            buyValue: 20, useable: true, equipable: false, 
                                            rarity: "Common", description: "Restores 10 health", healthincrease: 10);

            // Miscellaneous
            Item ShotgunBarrel  = new Item(itemtype: 5, name: "Shotgun Barrel", weight: 1, sellValue: 300, rarity: "Legendary", 
                                           description: "Can be crafted into shotgun");            
            Item ShotgunTrigger = new Item(itemtype: 5, name: "Shotgun Trigger", weight: .25f, sellValue: 300, rarity: "Legendary",
                                           description: "Can be crafted into shotgun");
            Item ShotgunStock   = new Item(itemtype: 5, name: "Shotgun Stock", weight: 1, sellValue: 300, rarity: "Legendary",
                                           description: "Can be crafted into shotgun");

            // Group items into rarity lists
            List<ItemI> commonItems = new List<ItemI> { PoolStick, WoodenBoard, RustyPipe, KitchenKnife, RustyShovel, BrokenBottle, BagOfGlassShards, BobbyPinBox, DamagedCrowbar, CargoShorts, CannedSoup, WaterBottle, IkeaMeatball, Gum, FannyPack, CanvasBag };
            List<ItemI> uncommonItems = new List<ItemI> { RustySword, RustedMachete, Lockpicks, Hammer, Bandages, Painkillers, BasicBackpack, KevlarJacket, TacticalShirt };
            List<ItemI> rareItems = new List<ItemI> { Crossbow, BarbedBaseballBat, FlareGun, Taser, RustyCrowbar, EnergyDrink, AdventureBag, OutdoorRucksack, ReinforcedArmor, BulletproofVest };
            List<ItemI> epicItems = new List<ItemI> { SharpenedKatana, RustyPistol, MolotovCocktail, Antidote, ExplorersBackpack, LeatherJacket, TacticalVest, Flashlight };
            List<ItemI> legendaryItems = new List<ItemI> { SwordOfNull, StrangeCharm, Burger, TitanStoragePack, GrandExplorersBackpack, MegaStorageBag, CombatArmor, ShotgunBarrel, ShotgunTrigger, ShotgunStock };

            Decorator.RarityDecorator DecoratedSwordOfNull = new Decorator.RarityDecorator(SwordOfNull, "Rare");

            var itemList = new List<Item> 
            { 
            SwordOfNull, SharpenedKatana, RustyPistol, MolotovCocktail, Crossbow, BarbedBaseballBat, FlareGun, Taser, RustySword, RustedMachete, 
            PoolStick, WoodenBoard, RustyPipe, KitchenKnife, RustyShovel, BrokenBottle, BagOfGlassShards, BobbyPinBox, DamagedCrowbar, Lockpicks, 
            Hammer, RustyCrowbar, KevlarJacket, Flashlight, StrangeCharm, CargoShorts, Burger, Antidote, EnergyDrink, Bandages, Painkillers, CannedSoup, WaterBottle, 
            IkeaMeatball, Gum, TitanStoragePack, GrandExplorersBackpack, MegaStorageBag, ExplorersBackpack, AdventureBag, OutdoorRucksack, BasicBackpack, 
            CanvasBag, FannyPack, CombatArmor, LeatherJacket, TacticalShirt, ReinforcedArmor, BulletproofVest, TacticalVest, ShotgunBarrel, ShotgunTrigger, ShotgunStock
            };

            foreach (var item in itemList)
            {
                _items[item.Name] = item;
            }            

            // logic for adding item into room
            Random random = new Random();
            int quantityNum = random.Next(3, 5);      // picks ran num 2-5 to pick num of items spawn in room
            //int quantityNum = 500;  // spawn every item switch with above to activate
            
            for (int f = 0; f < quantityNum; f++)
            {
                int rarityNum = random.Next(1, 101); // picks ran num 1-100 to pick what rarity would spawn
                if (rarityNum <= 45) // Common
                {
                    AddItemToCurrentRoom(commonItems);
                }
                else if (rarityNum <= 70) // Uncommon
                {
                    AddItemToCurrentRoom(uncommonItems);
                }
                else if (rarityNum <= 85) // Rare
                {
                    AddItemToCurrentRoom(rareItems);
                }
                else if (rarityNum <= 95) // Epic
                {
                    AddItemToCurrentRoom(epicItems);
                }
                else // Legendary
                {
                    AddItemToCurrentRoom(legendaryItems);
                }
            }

            return result;
        }

        public void AddItemToCurrentRoom(List<ItemI> itemList)
        {
            if (currentPlayer?.CurrentRoom != null)
            {
                Room room = currentPlayer.CurrentRoom;
                int ranIndex = new Random().Next(itemList.Count);
                ItemI getRanItem = itemList[ranIndex];
                Item? item = FindItem(getRanItem.Name);
                if (item != null)
                {
                    room.AddItem(item);
                }
            }
        }

        public Boolean AddKeyToRandomRoomInStage(Item keyName, string stageName)
        {
            Boolean result = false;
            List<string> roomGroup;
            RoomInfo roominfo = new RoomInfo();
            switch (stageName)
            {
                case "Stage1":
                    roomGroup = roominfo.defaultRoom.Take(14).ToList();
                    break;
                case "Stage2":
                    roomGroup = roominfo.abandonedUtilityHalls.Take(14).ToList();
                    break;
                case "Stage3":
                    roomGroup = roominfo.infiniteApartments.Take(14).ToList();
                    break;
                case "Stage4":
                    roomGroup = roominfo.poolRoom.Take(14).ToList();
                    break;
                case "Stage5":
                    roomGroup = roominfo.ikeaRoom.Take(14).ToList();
                    break;
                case "Stage6":
                    roomGroup = roominfo.arcadeRoom.Take(14).ToList();
                    break;
                case "Stage7":
                    roomGroup = roominfo.snackRoom.Take(14).ToList();
                    break;
                case "Stage8":
                    roomGroup = roominfo.carPark.Take(14).ToList();
                    break;
                default:
                    Console.WriteLine("Invalid Stage, Try Again!");
                    return false;
            }
            Shuffle(roomGroup);
            string rndRoom = roomGroup[0];
            Room? room = FindRoom(rndRoom);
            room?.AddItem(keyName);
            return result;
        }

        public Boolean Craft(string itemName, string? itemNameExt, string? itemNameExt2)
        {
            Boolean result = false;
            Player? player = currentPlayer;

            itemName = $"{itemName} {itemNameExt} {itemNameExt2}".Trim();

            if (player != null)
            {
            Inventory? inventory = FindInventory(player.Name);
            switch(itemName)
            {
                case "Shotgun":
                    Item? item1 = FindItemInInventory("Shotgun Barrel");
                    Item? item2 = FindItemInInventory("Shotgun Trigger");
                    Item? item3 = FindItemInInventory("Shotgun Stock");
                    if (item1 != null && item2 != null && item3 != null)
                    {
                        Item? Shotgun = new Item(itemtype: 0, name: "Shotgun", weight: 6, sellValue: 1000, 
                                    buyValue: null, useable: false, equipable: true, rarity: "Legendary",
                                        description: "description", damageamount: 50);
                        inventory?.AddItem(Shotgun);
                        inventory?.RemoveItem(item1);
                        inventory?.RemoveItem(item2);
                        inventory?.RemoveItem(item3);
                        Console.WriteLine($"You crafted a 'Shotgun' !");
                        break;
                    }
                    Console.WriteLine($"Items not found!");
                    break;
                default:
                    Console.WriteLine($"'{itemName}' is invalid!");
                    break;
                }           
            }
            return result;
        }

        public Boolean UseItem(string itemName, string? itemNameExt, string? itemNameExt2)
        {
            Boolean result = false;
            Player? player = currentPlayer;
            

            itemName = $"{itemName} {itemNameExt} {itemNameExt2}".Trim();

            if (player != null)
            {
                Inventory? inventory = FindInventory(player.Name);
                Item? item = FindItemInInventory(itemName);
                if (item?.Useable == true)
                {
                    switch (item.ItemType)
                    {
                        case 1:  // Tools
                            switch (item?.Name)
                            {
                                case "Flashlight":
                                    if (item.Uses != 0)
                                    {
                                        ItemCreation();
                                        item.Uses--;
                                        CurrentRoomDescription();
                                        Console.WriteLine($"Flashlight has {item.Uses} left!");
                                    }
                                    if (item.Uses == 0)
                                    {
                                        inventory?.RemoveItem(item);
                                        Console.WriteLine("Flashlight vanished from inventory!");
                                    }
                                    break;
                                case "Strange Charm":
                                    player.AttackDamage += 10;
                                    Console.WriteLine("A strange aura surrounds you!");
                                    Console.WriteLine("--- +10 Attack Damage ---");
                                    inventory?.RemoveItem(item);
                                    break;
                            }
                            break;
                        case 4:  // Consumable
                            player.Health += item.HealthIncrease;
                            Console.WriteLine("+" + item.HealthIncrease + " Health");
                            if (player.Health > 100)
                            {
                                player.Health = 100;
                                Console.WriteLine($"'{player.Name}' has healed to MAX HEALTH!");
                                break;
                            }
                            Console.WriteLine($"'{player.Name}' has healed to {player.Health}!");
                            inventory?.RemoveItem(item);
                            break;
                        case 5:  // Miscellaneous
                        
                            break;
                        default:
                            Console.WriteLine($"'{item.Name}' cannot be used");
                            break;
                        
                    }
                    return result;
                }
                Console.WriteLine($"{item?.Name} is not Useable");
                return result;
            }
            return result;
        }

        public Boolean EquipItem(string itemName, string? itemNameExt, string? itemNameExt2)
        {
            Boolean result = false;
            Player? player = currentPlayer;

            itemName = $"{itemName} {itemNameExt} {itemNameExt2}".Trim();

            if (player != null)
            {
                Item? item = FindItemInInventory(itemName);
                if (item?.Equipable == true)
                {
                    // write logic to equip item
                    switch (item?.ItemType)
                    {
                        case 0:  // Weapons
                            if (player.CurrentWeapon == null)
                            {
                                player.CurrentWeapon = item;
                                player.AttackDamage -= 3;
                                player.AttackDamage += item?.DamageAmount;
                                Console.WriteLine($"Equipped '{item?.Name}'");
                                break;
                            }
                            Console.WriteLine($"Please unequip '{player.CurrentWeapon.Name}' first!");
                            break;
                        case 2:  // Armor
                            if (player.CurrentArmor == null)
                            {
                                player.CurrentArmor = item;
                                player.Shield += item?.ShieldIncrease;
                                Console.WriteLine($"Equipped '{item?.Name}'");
                                break;
                            }
                            Console.WriteLine($"Please unequip '{player.CurrentArmor.Name}' first!");
                            break;
                        case 3:  // Backpacks
                            if (player.CurrentBackpack == null)
                            {
                                player.CurrentBackpack = item;
                                player.CurrentWeight += item?.InvIncrease;
                                Console.WriteLine($"Equipped '{item?.Name}'");
                                break;
                            }
                            Console.WriteLine($"Please unequip '{player.CurrentBackpack.Name}' first!");
                            break;
                        default:
                            Console.WriteLine($"'{item?.Name}' cannot be equipped!");
                            break;

                    }
                    return result;
                }
                Console.WriteLine($"{item?.Name} is not Equipable");
                return result;
            }
            return result;
        }

        public Boolean Unequip(string itemName, string? itemNameExt, string? itemNameExt2)
        {
            Boolean result = false;
            Player? player = currentPlayer;

            itemName = $"{itemName} {itemNameExt} {itemNameExt2}".Trim();

            if (player != null)
            {
                Item? item = FindItemInInventory(itemName);
                if (item?.Equipable == true)
                {
                    // write logic to equip item
                    switch (item?.ItemType)
                    {
                        case 0:  // Weapons
                            player.AttackDamage -= item.DamageAmount;
                            player.AttackDamage += 3;
                            player.CurrentWeapon = null;
                            Console.WriteLine($"Unequipped '{item?.Name}'");
                            break;
                        case 2:  // Armor
                            player.Shield -= item.ShieldIncrease;
                            player.CurrentArmor = null;
                            Console.WriteLine($"Unequipped '{item?.Name}'");
                            break;
                        case 3:  // Backpacks
                            player.CurrentWeight -= item.InvIncrease;
                            player.CurrentBackpack = null;
                            Console.WriteLine($"Unequipped  '{item?.Name}'");
                            break;
                        default:
                            Console.WriteLine($"'{item?.Name}' cannot be unequipped!");
                            break;
                    }
                }
            }  
            return result;
        }

        public Boolean DisplayAllItems()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("No items have been created.");
                return false;
            }

            Console.WriteLine("Created Items:");
            foreach (var item in _items.Values)
            {
                Console.WriteLine($"- {item.Name}: {item.Description}\nWeight: {item.Weight}\nSell Value: {item.SellValue}\nBuy Value: {item.BuyValue}" +
                $"\nUseable: {item.Useable}\nEquipable: {item.Equipable}\n");
            }
            return true;
        }

        private Item? FindItem(string itemName)
        {
            foreach (var item in _items.Values)
            {
                if (item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            Console.WriteLine($"Item '{itemName}' not found.");
            return null;
        }

        private Item? FindItemInInventory(string itemName)  
        {
            Player? player = currentPlayer;
            if (player != null)
            {
            Inventory? inventory = FindInventory(player.Name);
            Item? item = inventory?.GetItem(itemName);
            return item;
            }
            return null;
        }

        public Item? FindItemInCurrentRoom(string itemName)
        {
            Player? player = currentPlayer;
            Room? room = player?.CurrentRoom;
            Item? item = room?.GetItem(itemName);
            return item;
        }

        // ----------------------- NPC/Enemy Functions -------------------------------

        public Boolean SpawnNPC()
        {
            Boolean result = false;
            RoomInfo roominfo = new RoomInfo();
            Random random = new Random();
            // Hostile NPCS
            var bossesList = new List<NPC>
            {
                new NPC(name: "Default Boss", health: 65, attackdamage: 10, hostile: true, currentstage: "Stage 1"),
                new NPC(name: "Abandoned Utility Halls Boss", health: 80, attackdamage: 10, hostile: true, currentstage: "Stage 2"),
                new NPC(name: "Infinite Apartments Boss", health: 90, attackdamage: 15, hostile: true, currentstage: "Stage 3"),
                new NPC(name: "Pool Boss", health: 100, attackdamage: 20, hostile: true, currentstage: "Stage 4"),
                new NPC(name: "Ikea Boss", health: 120, attackdamage: 25, hostile: true, currentstage: "Stage 5"),
                new NPC(name: "Arcade Boss", health: 150, attackdamage: 30, hostile: true, currentstage: "Stage 6"),
                new NPC(name: "Snack Boss", health: 170, attackdamage: 35, hostile: true, currentstage: "Stage 7"),
                new NPC(name: "Car Park Boss", health: 200, attackdamage: 40, hostile: true, currentstage: "Stage 8")
            };

            // Stage 1 - Default
            var stage1Enemies = new List<NPC>
            {
                new NPC(name: "Leech", health: 15, attackdamage: 3, hostile: true, currentstage: "Stage 1"),
                new NPC(name: "Crawler", health: 20, attackdamage: 5, hostile: true, currentstage: "Stage 1"),
                new NPC(name: "Lurker", health: 25, attackdamage: 7, hostile: true, currentstage: "Stage 1")
            };

            // Stage 2 - Abandoned Utility Halls
            var stage2Enemies = new List<NPC>
            {
                new NPC(name: "Janitor", health: 30, attackdamage: 9, hostile: true, currentstage: "Stage 2"),
                new NPC(name: "Dust Bunny", health: 35, attackdamage: 11, hostile: true, currentstage: "Stage 2"),
                new NPC(name: "Dranged Mechanic", health: 40, attackdamage: 13, hostile: true, currentstage: "Stage 2")
            };

            // Stage 3 - Infinite Apartments
            var stage3Enemies = new List<NPC>
            {
                new NPC(name: "Landlord", health: 45, attackdamage: 15, hostile: true, currentstage: "Stage 3"),
                new NPC(name: "Karen", health: 40, attackdamage: 16, hostile: true, currentstage: "Stage 3"),
                new NPC(name: "Deranged Neighbor", health: 40, attackdamage: 18, hostile: true, currentstage: "Stage 3")
            };

            // Stage 4 - Pool
            var stage4Enemies = new List<NPC>
            {
                new NPC(name: "Fish Legs", health: 40, attackdamage: 18, hostile: true, currentstage: "Stage 4"),
                new NPC(name: "Crab Hands", health: 40, attackdamage: 18, hostile: true, currentstage: "Stage 4"),
                new NPC(name: "Shark Bait", health: 40, attackdamage: 19, hostile: true, currentstage: "Stage 4")
            };

            // Stage 5 - Ikea
            var stage5Enemies = new List<NPC>
            {
                new NPC(name: "Employee", health: 40, attackdamage: 19, hostile: true, currentstage: "Stage 5"),
                new NPC(name: "Manager", health: 40, attackdamage: 19, hostile: true, currentstage: "Stage 5"),
                new NPC(name: "Lost Customer", health: 45, attackdamage: 20, hostile: true, currentstage: "Stage 5")
            };

            // Stage 6 - Arcade
            var stage6Enemies = new List<NPC>
            {
                new NPC(name: "Pac Person", health: 45, attackdamage: 20, hostile: true, currentstage: "Stage 6"),
                new NPC(name: "Token Trooper", health: 45, attackdamage: 20, hostile: true, currentstage: "Stage 6"),
                new NPC(name: "Neon", health: 50, attackdamage: 22, hostile: true, currentstage: "Stage 6")
            };

            // Stage 7 - Snack
            var stage7Enemies = new List<NPC>
            {
                new NPC(name: "Chompy Chip", health: 50, attackdamage: 22, hostile: true, currentstage: "Stage 7"),
                new NPC(name: "Diet Doctor Sludge", health: 50, attackdamage: 22, hostile: true, currentstage: "Stage 7"),
                new NPC(name: "Popcorn Bomber", health: 55, attackdamage: 25, hostile: true, currentstage: "Stage 7")
            };

            // Stage 8 - Car
            var stage8Enemies = new List<NPC>
            {
                new NPC(name: "Turbo Tread", health: 55, attackdamage: 25, hostile: true, currentstage: "Stage 8"),
                new NPC(name: "Oil Slicker", health: 55, attackdamage: 25, hostile: true, currentstage: "Stage 8"),
                new NPC(name: "Crash Crawler", health: 60, attackdamage: 28, hostile: true, currentstage: "Stage 8")
            };

            var passiveList = new  List<NPC>
            {
                new NPC(name: "Shopkeeper", health: 10, attackdamage: 0, hostile: false, currentstage: "Stage 1"),
                new NPC(name: "Lost Explorer", health: 10, attackdamage: 0, hostile: false, currentstage: "Stage 1")
            };
            
    
            for (int i = 0; i < roominfo.bosses.Count; i++)
            {
                string bossRoomName = roominfo.bossroomNames[i];
                NPC boss = bossesList[i];

                Room? room = FindRoom(bossRoomName);
                room?.AddNPC(boss);
            }

            for (int i = 0; i < roominfo.shops.Count; i++)
            {
                string shopRoomName = roominfo.shops[i];
                NPC shopKeeper = passiveList[0];

                Room? room = FindRoom(shopRoomName);
                room?.AddNPC(shopKeeper);
            }
            
            var stageEnemies = new List<List<NPC>>
            {
                stage1Enemies, 
                stage2Enemies,
                stage3Enemies,
                stage4Enemies,
                stage5Enemies,
                stage6Enemies,
                stage7Enemies,
                stage8Enemies
            };

            // Loop through the stages and call AddEnemies
            for (int stageIndex = 0; stageIndex < stageEnemies.Count; stageIndex++)
            {
                AddEnemies(stageEnemies[stageIndex], stageIndex + 1);
            }


            return result;
        }

        public Boolean AddEnemies(List<NPC> npcList, int roomNameIndex)
        {
            Boolean result = false;
            Random random = new Random();
            RoomInfo roominfo = new RoomInfo();
            int quantityNum = random.Next(7, 13);
            for (int i = 0; i < quantityNum; i++)
            {
                int roomNum = random.Next(1, 17); 
                var randomEnemyIndex = random.Next(0, 3);
                Room? randomRoom = FindRoom(roominfo.roomNames[roomNameIndex] + roomNum);
                randomRoom?.AddNPC(npcList[randomEnemyIndex]);
            }

            return result;
        }

        public Boolean Fight(string npcName, string? npcNameExt, string? npcNameExt2)  // initilizes battle mode with said npc/enemy [yes you can attack npcs but thats just mean]
        {
            Boolean fightState = true;
            Player? player = currentPlayer;
            npcName = $"{npcName} {npcNameExt} {npcNameExt2}".Trim();
            NPC? npc = FindNPCInCurrentRoom(npcName);    
            Room? room = player?.CurrentRoom;

            if (player != null)
            {
                if (npc?.Health <= 0)
                {Console.WriteLine($"You cannot fight {npc?.Name}, it is dead."); fightState = false; return fightState;}
                Console.WriteLine($"Now Fighting '{npc?.Name}'\n\n---");
                while (fightState == true)
                {
                    Console.WriteLine($"{npc?.Name}'s Health: {npc?.Health}");
                    Console.WriteLine($"{player?.Name}'s Health: {player?.Health}\n{player?.Name}'s Shield: {player?.Shield}\n\nActions:\n---");
                    Console.WriteLine("1️⃣  Fight       2️⃣  Flee");
                    Console.WriteLine("3️⃣  Use Item    4️⃣  CheckInventory\n");
                    Console.Write    ("Enter a number to select an option: ");
                    int option = Convert.ToInt32(Console.ReadLine());
                    switch (option)
                    {
                        case 1:
                            if (npc != null)
                            {
                            npc.Health -= player?.AttackDamage;    
                            if (player != null) 
                            {
                                int? damageToPlayer = npc.AttackDamage;
                                if (player.Shield > 0)
                                {
                                    if (player.Shield >= damageToPlayer)
                                    {
                                        player.Shield -= damageToPlayer;
                                        damageToPlayer = 0;
                                    }
                                    else
                                    {
                                        damageToPlayer -= player.Shield;
                                        player.Shield =0;
                                    }
                                }
                                player.Health -= damageToPlayer;
                            }
                            Console.WriteLine("\n---\nCombat Result:");
                            Console.WriteLine($"You hit '{npc?.Name}' for {player?.AttackDamage} damage!");
                            Console.WriteLine($"'{npc?.Name}' hit you for {npc?.AttackDamage} damage!\n\n--"); }           
                            break;
                        case 2:
                            Random random = new Random();
                            int fleeChance = random.Next(0, 101);
                            if (fleeChance <= 55)
                            {
                                fightState = false;
                                Console.WriteLine("You've successfully fled!");
                            }
                            else
                            {
                                Console.WriteLine("You tripped and failed to flee!");
                                if (npc != null && player != null)
                                {
                                    int? damageToPlayer = npc.AttackDamage;

                                    if (player.Shield > 0)
                                    {
                                        if (player.Shield >= damageToPlayer)
                                        {
                                            player.Shield -= damageToPlayer;
                                            damageToPlayer = 0;
                                        }
                                        else
                                        {
                                            damageToPlayer -= player.Shield;
                                            player.Shield = 0;
                                        }
                                    }

                                    player.Health -= damageToPlayer;

                                    // Combat result display for the failed flee attempt
                                    Console.WriteLine("\n---\nCombat Result:");
                                    Console.WriteLine($"'{npc?.Name}' hit you for {npc?.AttackDamage} damage!");
                                    Console.WriteLine($"Your remaining Shield: {player?.Shield}");
                                    Console.WriteLine($"Your remaining Health: {player?.Health}\n\n---");
                                }
                            }
                            break;
                        case 3:
                            Console.Write("\nEnter the name of the item to use: ");
                            string? itemToUse = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(itemToUse))
                            {
                                Boolean usedItem = UseItem(itemToUse, null, null); 
                                Console.WriteLine("\n---");
                            }
                            break;
                        case 4:
                            if (player != null)
                            {
                                Console.WriteLine("Checking Inventory...");
                                Inventory? inventory = FindInventory(player.Name);
                                Console.WriteLine(inventory?.Description); }
                            break;     
                    }

                    if (npc?.Health <= 0)
                    {
                        Console.WriteLine($"You Killed '{npc.Name}'!");
                        CheckIfBossesBeat();
                        Console.WriteLine($"'{npc.Name}' dropping items...");
                        ItemCreation();
                        Console.WriteLine(room?.Description);
                        if (npc.Name.EndsWith("RoomsBoss"))
                        {
                            Console.WriteLine("Nice Job You beat a boss! Check out the [teleport] command!");
                            if (npc.Name == "CarParkBoss")
                            {
                                Console.WriteLine("Congratulations! You've escaped the Off-Brand-Backrooms!");
                                Environment.Exit(0);
                            }
                        }
                        fightState = false;
                    }
                    if (player?.Health <= 0)
                    {
                        Console.WriteLine($"You Died!");
                        Inventory? inventory = FindInventory(player.Name);
                        var itemsToDrop = inventory?.GetAllItems();
                        if (itemsToDrop != null)
                        foreach(Item item in itemsToDrop)
                        {
                            room?.AddItem(item);
                            inventory?.RemoveItem(item);
                        }
                        MoveToCheckpoint();
                        CheckIfBossesBeat();
                        fightState = false;
                    }
                }
            }
            return fightState;
        }
        
        public Boolean Talk(string npcName, string? npcNameExt, string? npcNameExt2)  // talk to NPCS and will prompt a bunch of conversations  you can talk to enemies but it'll end up
         // with a battle
        {
            Boolean result = false;
            Player? player = currentPlayer;
            npcName = $"{npcName} {npcNameExt} {npcNameExt2}".Trim();
            NPC? npc = FindNPCInCurrentRoom(npcName);
            Room? room = player?.CurrentRoom;
            

            if (player != null)
            {
                Inventory? inventory = FindInventory(player.Name);
                switch(npcName)
                {
                    case "Shopkeeper":
                        Boolean shopping = true;
                        while (shopping)
                        {
                            ItemInfo iteminfo = new ItemInfo();
                            iteminfo.printMenu();
                            Console.Write("Enter a number to select an item (or 0 to quit): ");
                            int option = Convert.ToInt32(Console.ReadLine());
                            if (option == 0) { Console.WriteLine("\nShopkeeper: Thanks for shopping Bye!"); shopping = false; }
                            Item? item = FindItem(iteminfo.itemList[option]);
                            Console.WriteLine("---");
                            Console.WriteLine("Name: " + item?.Name);
                            Console.WriteLine("Weight: " + item?.Weight);
                            Console.WriteLine("Buy Value: " + item?.BuyValue);
                            Console.WriteLine("Sell Value: " + item?.SellValue);
                            if (item?.DamageAmount != null) { Console.WriteLine("Damage Amount: " + item.DamageAmount); }
                            if (item?.InvIncrease != null) { Console.WriteLine("Inventory Increase: " + item.InvIncrease); }
                            if (item?.HealthIncrease != null) { Console.WriteLine("Health Increase: " + item.HealthIncrease); }
                            if (item?.ShieldIncrease != null) { Console.WriteLine("Shield Increase: " + item.ShieldIncrease); }
                            Console.WriteLine("\n---");
                            Console.WriteLine("1️⃣  Buy       2️⃣  Sell       3️⃣  Go Back");
                            Console.WriteLine($"Your balance is {player?.Scrap} scrap!\n");
                            Console.Write    ("Enter a number to select an option: ");
                            int option2 = Convert.ToInt32(Console.ReadLine());
                            switch (option2)
                            {
                                case 1:
                                    if (item?.BuyValue != null)
                                    {
                                        if (player?.Scrap >= item?.BuyValue)
                                        {
                                            int? itemValue = item?.BuyValue;
                                            if (player != null) 
                                            {
                                                player.Scrap -= itemValue;
                                                if (item != null) { inventory?.AddItem(item); }
                                                
                                            }
                                            Console.WriteLine($"\nYou bought, '{item?.Name}' for {itemValue} scrap!");
                                            Console.WriteLine($"Your balance is now {player?.Scrap} scrap!\n");
                                            Console.Write    ("Still Shopping? 1️⃣  Yes   2️⃣  No: ");
                                            int option3 = Convert.ToInt32(Console.ReadLine());
                                            switch (option3)
                                            {
                                                case 1:
                                                    break;
                                                case 2:
                                                    shopping = false;
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    if (item?.SellValue != null)
                                    {
                                        Item? item2 = FindItemInInventory(item.Name);
                                        if (item2 != null)
                                        {
                                            int? itemValue = item?.SellValue;
                                            if (player != null)
                                            {
                                                player.Scrap += itemValue;
                                                inventory?.RemoveItem(item2);
                                            }
                                            Console.WriteLine($"You sold, '{item?.Name}' for {itemValue} scrap!");
                                            Console.WriteLine($"Your balance is now {player?.Scrap} scrap!");
                                            Console.Write    ("Still Shopping? 1️⃣  Yes   2️⃣  No: ");
                                            int option3 = Convert.ToInt32(Console.ReadLine());
                                            switch (option3)
                                            {
                                                case 1:
                                                    break;
                                                case 2:
                                                    shopping = false;
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case 3:  // go back
                                    break;
                            }
                        }
                        Console.WriteLine(CurrentRoomDescription());
                        break;
                    case "Lost Explorer":
                        break;
                    case "Leech":
                        Console.WriteLine("\nSqueerchh!!!");
                        Fight("Leech", null, null);
                        break;
                    case "Crawler":
                        Console.WriteLine("\nRrraaaak!!!");
                        Fight("Crawler", null, null);
                        break;
                    case "Lurker":
                        Console.WriteLine("\nPsssst...");
                        Fight("Lurker", null, null);
                        break;
                    case "Janitor":
                        Console.WriteLine("\nI just cleaned the foor!!!!");
                        Fight("Janitor", null, null);
                        break;
                    case "Dust Bunny":
                        Console.WriteLine("\nWoosh....");
                        Fight("Dust Bunny", null, null);
                        break;
                    case "Dranged Mechanic":
                        Console.WriteLine("\nDid you steal my wrench?!?!!");
                        Fight("Deranged Mechanic", null, null);
                        break;
                    case "Landlord":
                        Console.WriteLine("\nGive me money!!!");
                        Fight("Landlord", null, null);
                        break;
                    case "Karen":
                        Console.WriteLine("\nWhere your manager!!!");
                        Fight("Karen", null, null);
                        break;
                    case "Deranged Neighbor":
                        Console.WriteLine("\nStop staring at me!!!");
                        Fight("Deranged Neighbor", null, null);
                        break;
                    case "Fish Legs":
                        Console.WriteLine("\nBlub Blub...");
                        Fight("Fish Legs", null, null);
                        break;
                    case "Crab Hands":
                        Console.WriteLine("\n*Clap, Clap*...");
                        Fight("Crab Hands", null, null);
                        break;
                    case "Shark Bait":
                        Console.WriteLine("\nChomp");
                        Fight("Shark bait", null, null);
                        break;
                    case "Employee":
                        Console.WriteLine("\nSir, no wepons allowed in the store.");
                        Fight("Employee", null, null);
                        break;
                    case "Manager":
                        Console.WriteLine("\nSir, please leave the store.");
                        Fight("Manager", null, null);
                        break;
                    case "Lost Customer":
                        Console.WriteLine("\nWHY CAN'T I FIND THE WAY OUT?!");
                        Fight("Lost Customer", null, null);
                        break;
                    case "Pac Person":
                        Console.WriteLine("\nWacka Wacka.");
                        Fight("Pac Person", null, null);
                        break;
                    case "Token Trooper":
                        Console.WriteLine("\nGive me your tokens.");
                        Fight("Token Trooper", null, null);
                        break;
                    case "Neon":
                        Console.WriteLine("\n.... . .-.. ---");
                        Fight("Neon", null, null);
                        break;
                    case "Chompy Chip":
                        Console.WriteLine("\nDon't eat me!!!");
                        Fight("Chompy Chip", null, null);
                        break;
                    case "Diet Doctor Sludge":
                        Console.WriteLine("\nSssssssss.");
                        Fight("Diet Doctor Sludge", null, null);
                        break;
                    case "Popcorn Bomber":
                        Console.WriteLine("\nPop Pop Pop.");
                        Fight("Popcorn Bomber", null, null);
                        break;
                    case "Turbo Tread":
                        Console.WriteLine("\nGota to burn rubber.");
                        Fight("Turbo Tread", null, null);
                        break;
                    case "Oil Slicker":
                        Console.WriteLine("\n...Hi.");
                        Fight("Oil Slicker", null, null);
                        break;
                    case "Crash Crawler":
                        Console.WriteLine("\nGet out of my way slow poke.");
                        Fight("Crash Crawler", null, null);
                        break; 
                }
            }
            return result;
        }

        public NPC? FindNPCInCurrentRoom(string npcName)
        {
            Player? player = currentPlayer;
            Room? room = player?.CurrentRoom;
            NPC? npc = room?.GetNPC(npcName);
            return npc;
        }

        public NPC? FindNPCInRoom(string npcName, string roomName)
        {
            Room? room = FindRoom(roomName);
            NPC? npc = room?.GetNPC(npcName);
            return npc;
        }

        public Boolean CheckIfBossesBeat()
        {
            Boolean result = false;
            RoomInfo roominfo = new RoomInfo();

            int maxLength = Math.Min(roominfo.bosses.Count, roominfo.bossroomNames.Count);

            for (int i = 0; i < maxLength; i++)
            {
                string bossName = roominfo.bosses[i];
                string roomName = roominfo.bossroomNames[i];

                // Find the NPC (boss) in the room
                NPC? npc = FindNPCInRoom(bossName, roomName);

                if (npc != null && npc.Health <= 0)  // Ensure NPC exists and is defeated
                {
                    string? npcStage = npc?.CurrentStage;

                    if (npcStage != null)
                    {
                        // Find the current stage associated with the defeated NPC
                        Stage? currentStage = FindStage(npcStage);

                        if (currentStage != null)
                        {
                            // Update the current stage state to "UNLOCKED"
                            currentStage.UpdateStageState("UNLOCKED");
                            Console.WriteLine($"{currentStage.Name} can now be teleported to");

                            // Attempt to unlock the next stage in the sequence
                            int currentIndex = roominfo.stageNames.IndexOf(npcStage);
                            if (currentIndex >= 0 && currentIndex < roominfo.stageNames.Count - 1)
                            {
                                string nextStageName = roominfo.stageNames[currentIndex + 1];
                                Stage? nextStage = FindStage(nextStageName);

                                if (nextStage != null)
                                {
                                    nextStage.UpdateStageState("UNLOCKED");
                                    Console.WriteLine($"{nextStage.Name} can now be teleported to as the next stage");
                                }
                            }

                            // Indicate that at least one stage was unlocked
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
        
        // ----------------------- Game Function -------------------------------------

        public Boolean NewGame()
        {
            Boolean result = false;
            Player? player = currentPlayer;
            

            // Story intro
            Console.WriteLine("You wake up in a mysterious place that looks like an office, but it’s been abandoned for years.");
            Console.WriteLine("As you explore, you discover cryptic messages hinting at the need to defeat terrifying bosses and survive whatever else lurks in the shadows.");
            Console.WriteLine("The only choice now is to face the unknown and navigate the eerie maze ahead.");

            RoomStageCreation();
            MapStages();
            PlacePlayerInRndLocation("Stage1");
            SpawnNPC();
            SetCheckpoint();
            if (_player.Count > 0)
            {
                if (player != null)
                {
                    Room? room = player.CurrentRoom;
                    if (room != null)
                    {
                        Room? bossRoom = FindRoom("DefaultBoss");
                        Room? shopRoom = FindRoom("DefaultShop");
                        if (bossRoom != null || shopRoom != null)
                        {
                            if (room.Name != bossRoom?.Name || room.Name != shopRoom?.Name)
                            {
                                ItemCreation();
                            }
                        }
                        Console.WriteLine($"Player '{player.Name}' is currently in room: {room.Name}");
                        Console.WriteLine(room.Description);
                        Console.WriteLine("\nHint: Use move <direction>");
                        result = true;
                        return result;
                    }
                }
                Console.WriteLine($"{player} not found.");
                return result;
            }
            return result;
        }

        // ----------------------- Game Function ------------------------
        public Boolean LoadGame(string saveName)
        {
            Boolean result = false;

            return result;
        }

        public Boolean SaveGame(string saveName)
        {
            Boolean result = false;

            return result;
        }
        
        // ----------------------- Room Creation / Stage Creation ------------------------

        public Boolean RoomStageCreation() 
        {
            Boolean result = false;
            RoomInfo roominfo = new RoomInfo();
            List<string> roomGroup1 = roominfo.defaultRoom;

            // Room 2: Abandoned Utility Halls
            List<string> roomGroup2 = roominfo.abandonedUtilityHalls;

            // Room 3: Infinite Apartments
            List<string> roomGroup3 = roominfo.infiniteApartments;

            // Room 4: Pool
            List<string> roomGroup4 = roominfo.poolRoom;

            // Room 5: Ikea
            List<string> roomGroup5 = roominfo.ikeaRoom;

            // Room 6: Arcade
            List<string> roomGroup6 = roominfo.arcadeRoom;

            // Room 7: Snack Room
            List<string> roomGroup7 = roominfo.snackRoom;

            // Room 8: Car Park
            List<string> roomGroup8 = roominfo.carPark;

            List<List<string>> roomGroups = new List<List<string>> 
            {
                roomGroup1, roomGroup2, roomGroup3, roomGroup4, 
                roomGroup5, roomGroup6, roomGroup7, roomGroup8
            };

            List<string> allRooms = new List<string>();

            foreach (var roomGroup in roomGroups)
            {
                Shuffle(roomGroup);
                allRooms.AddRange(roomGroup);
            }

            for (int i = 1; i <= 8; i++)
            {
                CreateStage($"Stage {i}", "LOCKED");
            }

            foreach (string roomName in allRooms)
            {
                CreateRoom(roomName);
            }

            int roomCount = 1;
            foreach (string roomName in allRooms)
            {

                if (roomCount >= 1 && roomCount <= 16)
                {
                    AddRoomToStage(roomName, "Stage 1");
                }
                else if (roomCount >= 17 && roomCount <= 32)
                {
                    AddRoomToStage(roomName, "Stage 2");
                }
                else if (roomCount >= 33 && roomCount <= 48)
                {
                    AddRoomToStage(roomName, "Stage 3");
                }
                else if (roomCount >= 49 && roomCount <= 64)
                {
                    AddRoomToStage(roomName, "Stage 4");
                }
                else if (roomCount >= 65 && roomCount <= 80)
                {
                    AddRoomToStage(roomName, "Stage 5");
                }
                else if (roomCount >= 81 && roomCount <= 96)
                {
                    AddRoomToStage(roomName, "Stage 6");
                }
                else if (roomCount >= 97 && roomCount <= 112)
                {
                    AddRoomToStage(roomName, "Stage 7");
                }
                else if (roomCount >= 113 && roomCount <= 129)
                {
                    AddRoomToStage(roomName, "Stage 8");
                }
                roomCount++;
            }        
            return result;
        }

        // ----------------------- Stage Function ------------------------

        public Boolean CreateStage(string name, string stagestate)
        {
            Boolean result = false;
            if (!_stages.ContainsKey(name))
            {
                _stages[name] = new Stage(name, stagestate);
                return result;
            }
            return result;
        }

        public Boolean AddRoomToStage(string roomName, string stageName)
        {
            Boolean result = false;
            
            Room? room = FindRoom(roomName);
            if (room != null)
            {
                Stage? stage = FindStage(stageName);
                    stage?.AddRoom(room);
                    result = true;
                    return result;
            }
            Console.WriteLine("room not found");
            return result;
        }

        public Boolean MapStages()
        {
            Boolean result = false;
            for (int i = 1; i <= 8; ++i)
            {
                Stage? stage = FindStage($"Stage {i}");
                    stage?.MapStage(4, 4);
            }
            result = true;
            return result;
        }

        private Stage? FindStage(string stageName)
        {
            Stage? stage = null;
            _stages.TryGetValue(stageName, out stage);
            return stage;
        }

        public Boolean ListStageStates()
        {
            Boolean result = false;
            RoomInfo roominfo = new RoomInfo();
            CheckIfBossesBeat();

            int maxLength = Math.Min(roominfo.bosses.Count, roominfo.bossroomNames.Count);

            for (int i = 0; i < maxLength; i++)
            {
                string bossName = roominfo.bosses[i];
                string roomName = roominfo.bossroomNames[i];
                NPC? npc = FindNPCInRoom(bossName, roomName);
                string? npcStage = npc?.CurrentStage;
                if (npcStage != null) 
                {
                    Stage? stage = FindStage(npcStage);
                    Console.WriteLine(npc?.Name);
                    Console.WriteLine(npcStage);
                    Console.WriteLine(stage?.StageState);
                }
                
                
            }
            return result;
        }           


        // ----------------------- Rooms Function ------------------------
        public Boolean CreateRoom(string name)
        {
            Boolean result = false;
            if (!_rooms.ContainsKey(name))
            {
                _rooms[name] = new Room(name);
                result = true;
                return result;
            }
            return result;
        }

        public Boolean AddPlayerToRoom(string roomName)
        {
            Boolean result = false;
            Player? player = currentPlayer;
            if (player != null)
            {
                Room? room = FindRoom(roomName);
                if (room != null)
                {
                    result = room.AddPlayer(player);
                    if (result)
                    {
                        player.CurrentRoom = room;
                    }
                }
            }
            return result;
        }

        public Boolean RemovePlayerFromRoom(string roomName)
        {
            Boolean result = false;
            Player? player = currentPlayer;
            if (player != null)
            {
                Room? room = FindRoom(roomName);
                if (room != null)
                {
                    result = room.RemovePlayer(player);
                    if (result)
                    {
                        player.CurrentRoom = null;
                    }
                }
            }
            return result;
        }

        public Boolean CurrentRoomDescription()
        {
            Boolean result = false;
            Player? player = currentPlayer;
            if (player != null)
            {
                Room? room = player.CurrentRoom;
                Console.WriteLine(room?.Description);
            }         
            return result;
        }

        private Room? FindRoom(string roomName)
        {
            Room? room = null;
            _rooms.TryGetValue(roomName, out room);
            return room;
        }
        
        // ----------------------- List All Commands -------------------------------
        public Boolean ListAllCommands()
        {
            Boolean result = false;
            Console.WriteLine("\n1.  move <direction>");
            Console.WriteLine("2.  moveBack ");
            Console.WriteLine("3.  pickup <itemName>");
            Console.WriteLine("4.  drop   <itemName>");
            Console.WriteLine("5.  checkinventory");
            Console.WriteLine("6.  equip  <itemName>");
            Console.WriteLine("7.  use    <itemName>");
            Console.WriteLine("8.  playerstats");
            Console.WriteLine("9.  fight <entityName>");
            Console.WriteLine("10. talk <entityName>");
            Console.WriteLine("11. teleport");
            Console.WriteLine("12. CRD (current room description)");
            Console.WriteLine("13. exit");

            Console.WriteLine("\nAdmin Commands 'LAR LAC TD CRD LSS'");
            return result;
        }

        // ----------------------- List ALl Rooms Function ------------------------
        public Boolean ListAllRooms()
        {
            Boolean result = false;
            Console.WriteLine("Listing all rooms: \n");
            foreach(Room room in _rooms.Values)
            {
                Console.WriteLine(room.Name);
            }
            result = true;
            return result;
        }

        // ----------------------- Other Functions -------------------------------
        static Random rng = new Random();
        public static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}