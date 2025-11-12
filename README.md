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

## 🎮 Commands

Common commands (exact spellings may vary by implementation):

| Command | Description |
|----------|-------------|
| `look` | Describe the current room |
| `move <direction>` | Move to an adjacent room |
| `inventory` | Show your inventory |
| `pickup <item>` | Pick up an item |
| `drop <item>` | Drop an item |
| `equip <item>` | Equip a weapon or item |
| `unequip <item>` | Unequip the current item |
| `use <item>` | Use a consumable or tool |
| `talk <npc>` | Speak with an NPC |
| `fight <npc>` | Initiate combat |
| `craft <item>` | Craft an item from components |
| `help` | List available commands |

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
