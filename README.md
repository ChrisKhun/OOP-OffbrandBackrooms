# 🌀 Offbrand Backrooms

**Offbrand Backrooms** is a text-based adventure survival game inspired by the eerie *Backrooms* concept — written entirely in **C# (.NET 8)**.  
Explore endless, unsettling rooms, interact with strange NPCs, collect and craft items, and try to survive the Offbrand Backrooms.

---

## 🧠 Overview

Offbrand Backrooms recreates the nostalgic feel of classic console exploration games with a modern, object-oriented architecture.  
Gameplay revolves around **typed commands**, each mapped to dedicated C# classes handling movement, combat, inventory, crafting, and dialogue.

**Highlights**
- Encapsulation of player, room, and item logic  
- Inheritance & polymorphism across the command system  
- Interface-driven design for entities (e.g., `ItemI`, `NPCI`)  
- **Decorator pattern** for layered item effects  

---

## ⚙️ Features

- 🎮 **Command-Based Gameplay** — move, fight, craft, talk, etc.  
- 🧰 **Inventory System** — pick up, drop, equip, use, and craft items  
- 🗺️ **Room Exploration** — navigate interconnected rooms with unique descriptions  
- 🧍 **NPC Interactions** — talk to or fight mysterious entities  
- 🔧 **Crafting** — combine materials to create tools and gear  
- ⚔️ **Combat** — text-based fights with health/attack stats  
- 🧱 **Decorator Pattern** — stackable item effects (e.g., enhanced attack)  
- 🧩 **Extensible Architecture** — easily add commands, items, rooms, or NPCs  

---

## 📁 Project Structure

    Coding Project/
    │
    ├── OffBrandBackrooms.sln
    ├── .vscode/
    │   └── settings.json
    │
    └── OffBrandBackrooms/
        ├── Program.cs                      # Entry point
        ├── Command.cs / CommandFunctions.cs
        ├── Parser.cs                       # Input → command routing
        ├── MovePlayer.cs / Talk.cs / Fight.cs / Craft.cs / Teleport.cs
        ├── Equip.cs / Unequip.cs / DropItem.cs / CheckInventory.cs
        ├── Player.cs / PlayerStats.cs / CreatePlayer.cs
        ├── Inventory.cs / Item.cs / ItemInfo.cs / Useable.cs / Decorator.cs
        ├── Room.cs / RoomInfo.cs / Stage.cs / CurrentRoomDescription.cs
        ├── NPC.cs / NPCI.cs
        ├── StartGame.cs / ListAllCommands.cs / ListAllRooms.cs
        ├── Program Planning Doc.txt
        ├── OffBrandBackrooms.csproj
        └── bin/Debug/net8.0/               # Compiled output

---

## 🚀 Getting Started

### Requirements
- **.NET SDK 8.0+** (Windows, macOS, or Linux)

### Build & Run
    cd "Coding Project/OffBrandBackrooms"
    dotnet build
    dotnet run
    # Or run compiled binary:
    # ./bin/Debug/net8.0/OffBrandBackrooms.exe

---

## 🎮 Player Commands & Usage

Below is a full list of supported **commands**, their **usage**, and **in-game purpose**.

| Command | Usage Example | Description |
|----------|---------------|-------------|
| `move <direction>` | `move north` | Moves the player to a connected room (north, south, east, or west). |
| `moveBack` | `moveBack` | Returns the player to the previous room in history. |
| `pickup <itemName>` | `pickup key` | Adds the specified item to your inventory. |
| `drop <itemName>` | `drop flashlight` | Removes an item from inventory and drops it in the current room. |
| `checkinventory` | `checkinventory` | Lists all items in your inventory and their stats. |
| `equip <itemName>` | `equip pistol` | Equips an item as a weapon or tool. |
| `unequip <itemName>` | `unequip pistol` | Unequips the currently equipped weapon or tool. |
| `use <itemName>` | `use medkit` | Uses a consumable item such as health packs or keys. |
| `playerstats` | `playerstats` | Displays current player stats: health, shield, scrap, damage, weight, and weapon. |
| `talk <npcName>` | `talk shopkeeper` | Interacts with an NPC. May open dialogue or a shop. |
| `fight <npcName>` | `fight lurker` | Engages the specified NPC in combat. |
| `craft <itemName>` | `craft crowbar` | Combines materials to create new tools or weapons. |
| `teleport` | `teleport` | Teleports to unlocked stages after defeating certain bosses. |
| `CRD` | `CRD` | Prints the description of your current room (Current Room Description). |
| `LAR` | `LAR` | Lists all rooms in the current world/stage. |
| `LAC` | `LAC` | Lists all available commands in-game (shows this list). |
| `LSS` | `LSS` | Lists all stage states and boss statuses. |
| `exit` | `exit` | Quits the game safely. |


> 💡 **Tip:** Explore, gather, and craft early to survive tougher encounters.

---

## 🧱 Technical Notes

- **OOP architecture:** modular systems for rooms, items, inventory, NPCs, and player stats  
- **Command parser:** `Parser.cs` routes input to command classes  
- **Interfaces:** `ItemI`, `NPCI`, etc., promote loose coupling and testability  
- **Decorator pattern:** `Decorator.cs` enables layered item effects  
- **Extensibility:** add a new command by creating a `*Command*.cs` and registering it in the parser/command table  

---

## 🧑‍💻 Development Environment

- **Language:** C#  
- **Runtime:** .NET 8  
- **Editors:** Visual Studio 2022 / VS Code  
- **OS:** Windows / macOS / Linux (console)  

---

## 🏁 Roadmap

- Procedural room generation & ambient events  
- Save/load system (JSON serialization)  
- Expanded NPC dialogue trees and trading  
- Additional item effects & status ailments  
- Difficulty modes and accessibility options  

---

## 👥 Credits

**Author:** Christopher Khun  
Special thanks to classmates and instructors for feedback on command design, data structures, and gameplay flow.

---

## 📜 License

This project is provided as-is for educational / portfolio use.  
