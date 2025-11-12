using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class NPC : NPCI
    {
        private string _name = "";
        private int? _health = 0;
        private int? _attackdamage = 0;
        private Boolean _hostile = false;

        // Public properties for accessing and modifying private fields
        public string Name { get { return _name; } set { _name = value; } }
        public int? Health { get { return _health; } set { _health = value; } }
        public int? AttackDamage { get { return _attackdamage; } set { _attackdamage = value; } }
        public Boolean Hostile { get { return _hostile; } set { _hostile = value; } }
        public string? CurrentStage { get; set; }

        // default constructor
        public NPC() : this("NO NAME", 0, 0, false, "NO STAGE") { }

        public NPC (string name, int? health, int? attackdamage, bool hostile, string currentstage)
        {
            Name = name;
            Health = health;
            AttackDamage = attackdamage;
            Hostile = hostile;
            CurrentStage = currentstage;
        }
        override
        public string ToString()
        {
            return Name + ", " + Health + ", " + AttackDamage;
        }
    }
}