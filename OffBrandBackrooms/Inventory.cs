using System;
using System.Collections.Generic;

namespace OffBrandBackrooms
{
    public class Inventory
    {
        public string InventoryName { get; set; }
        private List<Item> _items;  

        public Inventory() : this("NAMELESS") { }

        public Inventory(string inventoryName)
        {
            InventoryName = inventoryName;
            _items = new List<Item>();  
        }

        // Add an item to the inventory, allowing duplicates
        public Boolean AddItem(Item item)
        {
            _items.Add(item);  
            return true;
        }

        // Remove an item from the inventory
        public Boolean RemoveItem(Item item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item); 
                return true;
            }
            return false;
        }

        // description of the inventory
        public string Description
        {
            get
            {
                if (_items.Count == 0)
                {
                    return $"Inventory for {InventoryName} is empty.";
                }

                string output = $"Inventory for {InventoryName}:\n";
                var groupedItems = new Dictionary<string, int>();

                // Count how many times each item appears
                foreach (var item in _items)
                {
                    if (groupedItems.ContainsKey(item.Name))
                    {
                        groupedItems[item.Name]++;
                    }
                    else
                    {
                        groupedItems[item.Name] = 1;
                    }
                }

                // Describe each unique item
                foreach (var item in groupedItems)
                {
                    var itemName = item.Key;
                    var count = item.Value;
                    var firstItem = _items.Find(i => i.Name == itemName);

                    if (firstItem != null)
                    {
                        output += $"\n- {itemName} (x{count}):\n" +
                                $"  Weight       : {firstItem.Weight}\n" +
                                $"  Sell Value   : {firstItem.SellValue}\n" +
                                $"  Buy Value    : {firstItem.BuyValue?.ToString() ?? "N/A"}\n" +
                                $"  Useable      : {firstItem.Useable}\n" +
                                $"  Equipable    : {firstItem.Equipable}\n" +
                                $"  Rarity       : {firstItem.Rarity}\n" +
                                $"  Description  : {firstItem.Description}\n" +
                                $"  Damage Amount: {firstItem.DamageAmount}\n" +
                                $"  Heal Amount  : {firstItem.HealthIncrease}\n" +
                                $"  Shield Increase : {firstItem.ShieldIncrease}\n" +
                                $"  Inventory Increase : {firstItem.InvIncrease}\n" +
                                $"  Remaining Uses : {firstItem.Uses}\n";
                    }
                }

                return output;
            }
        }


        // Method to find if an item exists by name
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
        // Get an item by name (returns null if not found)
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

        // Method to retrieve all items in the inventory
        public List<Item> GetAllItems()
        {
            return new List<Item>(_items); // Return a copy of the list to avoid external modifications
        }

    }
}
