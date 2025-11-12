using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class Player : PlayerI    
    {
        private string _name = "";
        private string _gender = "";
        private int? _health = 0;
        private int? _shield = 0;
        private int? _scrap = 0;
        private int? _attackdamage = 0;
        private float? _currentweight = 0;
        public string Name { get { return _name; } set { _name = value; } }
        public string Gender { get { return _gender; } set { _gender = value; } }
        public int? Health { get { return _health; } set { _health = value; } }
        public int? Shield { get { return _shield; } set { _shield = value; } }
        public int? Scrap { get { return _scrap; } set { _scrap = value; } }
        public int? AttackDamage { get { return _attackdamage; } set { _attackdamage = value; } }
        public float? CurrentWeight { get { return _currentweight; } set { _currentweight = value; } }
        public Item? CurrentArmor { get; set; }
        public Item? CurrentWeapon { get; set; }
        public Item? CurrentBackpack { get; set; }
        public Room? CurrentRoom { get; set; }
        public Room? Checkpoint { get; set; }


        public Player() : this("NO NAME", "NO GENDER", 0, 0, 0, 0, 0) { }

        // Designated Constructor
        public Player(string name, string gender, int? health, int? shield, int? scrap, float? currentweight, int? attackdamage)
        {
            Name = name;
            Gender = gender;
            Health = health;
            Shield = shield;
            Scrap = scrap;
            AttackDamage = attackdamage;
            CurrentWeight = currentweight;
            CurrentArmor = null;
            CurrentWeapon = null;
            CurrentBackpack = null;
            CurrentRoom = null;
            Checkpoint = null;
        }
        override
        public string ToString()
        {
            return Name + ", " + Gender;
        }
    }
}