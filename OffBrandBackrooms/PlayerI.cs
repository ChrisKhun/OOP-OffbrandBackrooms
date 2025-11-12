using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public interface PlayerI
    {
        string Name { get; set; }
        string Gender { get; set; }
        int? Health { get; set; }
        int? Shield { get; set; }
        int? Scrap { get; set; }
        int? AttackDamage { get; set; }
        float? CurrentWeight { get; set; }
        string ToString();
    }
}
