using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public interface ItemI
    {
        int ItemType {get; set; }
        string Name { get; set; }
        float Weight { get; set; }
        int SellValue { get; set; }  
        int? BuyValue { get; set; }  
        public Boolean Useable { get; set; }
        public Boolean Equipable { get; set; }
        public string Rarity { get; set; }
        string Description { get; }    
        int? DamageAmount { get; set; }
        float? InvIncrease { get; set; }
        int? HealthIncrease { get; set; }
        int? ShieldIncrease { get; set; }

        void AddDecorator(ItemI item);
    }
}