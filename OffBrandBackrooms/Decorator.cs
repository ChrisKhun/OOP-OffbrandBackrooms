using System;
using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class Decorator : ItemI
    {
        protected ItemI _item;

        public Decorator(ItemI item)
        {
            _item = item;
        }

        public virtual int ItemType
        {
            get => _item.ItemType;
            set => _item.ItemType = value;
        }
    
        public virtual string Name
        {
            get => _item.Name;
            set => _item.Name = value;
        }

        public virtual float Weight
        {
            get => _item.Weight;
            set => _item.Weight = value;
        }

        public virtual int SellValue
        {
            get => _item.SellValue;
            set => _item.SellValue = value;
        }

        public virtual int? BuyValue
        {
            get => _item.BuyValue;
            set => _item.BuyValue = value;
        }

        public virtual Boolean Useable
        {
            get => _item.Useable;
            set => _item.Useable = value;
        }

        public virtual Boolean Equipable
        {
            get => _item.Equipable;
            set => _item.Equipable = value;
        }

        public virtual string Rarity
        {
            get => _item.Rarity;
            set => _item.Rarity = value;
        }

        public virtual string Description => _item.Description;
        
        public virtual void AddDecorator(ItemI item)
        {
            _item.AddDecorator(item);
        }

        public virtual int? DamageAmount
        {
            get => _item.DamageAmount;
            set => _item.DamageAmount = value;
        }

        public virtual float? InvIncrease
        {
            get => _item.InvIncrease;
            set => _item.InvIncrease = value;
        }

        public virtual int? HealthIncrease
        {
            get => _item.HealthIncrease;
            set => _item.HealthIncrease = value;
        }

        public virtual int? ShieldIncrease
        {
            get => _item.ShieldIncrease;
            set => _item.ShieldIncrease = value;
        }

        // rarity decorator
        public class RarityDecorator : Decorator
        {
            private readonly string _rarity;

            public RarityDecorator(ItemI item, string rarity) : base(item)
            {
                _rarity = rarity;
            }

            // Override the Rarity property
            public override string Rarity => _rarity;

            // Retain original description and append rarity details
            public override string Description => $"{_item.Description} [Rarity: {_rarity}]";
        }

        // curse of binding
        
        // damage buff

        // 
    }
}
