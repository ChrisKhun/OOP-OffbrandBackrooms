using System;
using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class Item : ItemI
    {
        private ItemI? _decorator;

        // Properties implementing ItemI
        public int ItemType { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public int SellValue { get; set; }
        public int? BuyValue { get; set; }
        public Boolean Useable { get; set; }
        public int? Uses { get; set; }
        public Boolean Equipable { get; set; }
        public string Rarity {get; set; }
        public string Description { get; }
        public int? DamageAmount { get; set; }
        public float? InvIncrease { get; set;}
        public int? HealthIncrease { get; set; }
        public int? ShieldIncrease { get; set; }

        public Item(int itemtype, string name, float weight, int sellValue, int? buyValue = null, 
                    bool useable = false, bool equipable = false, string rarity = "Common", 
                    string description = "No description.", int? damageamount = null, float? invincrease = null,
                    int? healthincrease = null, int? shieldincrease = null, int? uses = 3) // if var not set it would go to these default values
        {
            ItemType = itemtype;
            Name = name;
            Weight = weight;
            SellValue = sellValue;
            BuyValue = buyValue;
            Useable = useable;
            Equipable = equipable;
            Rarity = rarity;
            Description = description;
            DamageAmount = damageamount;
            InvIncrease = invincrease;
            HealthIncrease = healthincrease;
            ShieldIncrease = shieldincrease;
            Uses = uses;
        }

        // Corrected AddDecorator implementation without override
        public void AddDecorator(ItemI item)
        {
            if (_decorator == null)
            {
                _decorator = item;
            }
            else
            {
                // If there’s already a decorator, delegate to the next in the chain
                if (_decorator is Item decoratorItem)
                {
                    decoratorItem.AddDecorator(item);
                }
            }
        }
    }
}