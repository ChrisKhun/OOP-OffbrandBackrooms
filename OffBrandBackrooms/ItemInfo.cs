using System;
using System.Collections.Generic;
using System.Linq;

namespace OffBrandBackrooms
{
    public class ItemInfo
    {
        public List<string> itemList { get; set; } 
        
        public ItemInfo()
        {
            itemList = new List<string> 
            { 
                "ignore", "Sword of Null", "Sharpened Katana", "Rusty Pistol", "Molotov Cocktail", "Crossbow", "Barbed Baseball Bat", "Flare Gun", "Taser", 
                "Rusty Sword", "Rusted Machete", "Pool Stick", "Wooden Board", "Rusty Pipe", "Kitchen Knife", "Rusty Shovel", "Broken Bottle", "Bag of Glass Shards",
                "Strange Charm", "Rusty Crowbar", "Flashlight", "Lockpicks", "Hammer", "Bobby Pin Box", "Damaged Crowbar",
                "Combat Armor", "Leather Jacket", "Tactical Vest", "Reinforced Armor", "Bulletproof Vest", "Kevlar Jacket", "Tactical Shirt", "Sports Armor", 
                "Padded Shirt", "Dirty T-Shirt", "Titan Storage Pack", "Grand Explorer's Backpack", "Mega Storage Bag", "Explorer's Backpack", "Adventure Bag", 
                "Outdoor Rucksack", "Basic Backpack", "Cargo Shorts", "Canvas Bag", "Fanny Pack", "Burger", "Antidote", "Energy Drink", "Bandages", 
                "Painkillers", "Canned Soup", "Water Bottle", "Ikea Meatball", "Gum", "Shotgun Barrel", "Shotgun Trigger", "Shotgun Stock"


            };
        }

        

        public void printMenu()
        {
            Console.WriteLine("============================================ ITEM MENU ========================================");

            // Weapons & Tools
            Console.WriteLine($"{"--- Weapons ---", 40} {"--- Tools ---", 40}");
            Console.WriteLine($"1. Sword of Null                  (Legendary)      {"18. Strange Charm                (Legendary)",-40}");
            Console.WriteLine($"2. Sharpened Katana               (Epic)           {"19. Rusty Crowbar                 (Rare)",-40}");
            Console.WriteLine($"3. Rusty Pistol                   (Epic)           {"20. Flashlight                    (Rare)",-40}");
            Console.WriteLine($"4. Molotov Cocktail               (Epic)           {"21. Lockpicks                     (Uncommon)",-40}");
            Console.WriteLine($"5. Crossbow                       (Rare)           {"22. Hammer                        (Uncommon)",-40}");
            Console.WriteLine($"6. Barbed Baseball Bat            (Rare)           {"23. Bobby Pin Box                 (Common)",-40}");
            Console.WriteLine($"7. Flare Gun                      (Rare)           {"24. Damaged Crowbar               (Common)",-40}");
            Console.WriteLine($"8. Taser                          (Rare)           {"", -40}");
            Console.WriteLine($"9. Rusty Sword                    (Uncommon)       {"", -40}");
            Console.WriteLine($"10. Rusted Machete                (Uncommon)       {"", -40}");
            Console.WriteLine($"11. Pool Stick                    (Common)         {"", -40}");
            Console.WriteLine($"12. Wooden Board                  (Common)         {"", -40}");
            Console.WriteLine($"13. Rusty Pipe                    (Common)         {"", -40}");
            Console.WriteLine($"14. Kitchen Knife                 (Common)         {"", -40}");
            Console.WriteLine($"15. Rusty Shovel                  (Common)         {"", -40}");
            Console.WriteLine($"16. Broken Bottle                 (Common)         {"", -40}");
            Console.WriteLine($"17. Bag of Glass Shards           (Common)         {"", -40}");
            Console.WriteLine();

            // Armor & Backpacks
            Console.WriteLine($"{"--- Armor ---", 40} {"--- Backpacks ---", 40}");
            Console.WriteLine($"25. Combat Armor                  (Legendary)      {"35. Titan Storage Pack            (Legendary)",-40}");
            Console.WriteLine($"26. Leather Jacket                (Epic)           {"36. Grand Explorer's Backpack     (Legendary)",-40}");
            Console.WriteLine($"27. Tactical Vest                 (Epic)           {"37. Mega Storage Bag              (Legendary)",-40}");
            Console.WriteLine($"28. Reinforced Armor              (Rare)           {"38. Explorer's Backpack           (Epic)",-40}");
            Console.WriteLine($"29. Bulletproof Vest              (Rare)           {"39. Adventure Bag                 (Rare)",-40}");
            Console.WriteLine($"30. Kevlar Jacket                 (Uncommon)       {"40. Outdoor Rucksack              (Rare)",-40}");
            Console.WriteLine($"31. Tactical Shirt                (Uncommon)       {"41. Basic Backpack                (Uncommon)",-40}");
            Console.WriteLine($"32. Sports Armor                  (Common)         {"42. Cargo Shorts                  (Common)",-40}");
            Console.WriteLine($"33. Padded Shirt                  (Common)         {"43. Canvas Bag                    (Common)",-40}");
            Console.WriteLine($"34. Dirty T-Shirt                 (Common)         {"44. Fanny Pack                    (Common)",-40}");
            Console.WriteLine();

            // Consumables & Miscellaneous
            Console.WriteLine($"{"--- Consumables ---", 40} {"--- Miscellaneous ---", 40}");
            Console.WriteLine($"45. Burger                        (Legendary)      {"54. Shotgun Barrel                (Legendary)",-40}");
            Console.WriteLine($"46. Antidote                      (Epic)           {"55. Shotgun Trigger               (Legendary)",-40}");
            Console.WriteLine($"47. Energy Drink                  (Rare)           {"56. Shotgun Stock                 (Legendary)",-40}");
            Console.WriteLine($"48. Bandages                      (Uncommon)       {"",-40}");
            Console.WriteLine($"49. Painkillers                   (Uncommon)       {"",-40}");
            Console.WriteLine($"50. Canned Soup                   (Common)         {"",-40}");
            Console.WriteLine($"51. Water Bottle                  (Common)         {"",-40}");
            Console.WriteLine($"52. Ikea Meatball                 (Common)         {"",-40}");
            Console.WriteLine($"53. Gum                           (Common)         {"",-40}");
            Console.WriteLine();
        }
    }
}