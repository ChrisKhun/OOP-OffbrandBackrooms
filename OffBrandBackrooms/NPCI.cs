using System.Threading.Tasks;
using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public interface NPCI
    {
        string Name { get; set; }
        int? Health { get; set; }
        int? AttackDamage { get; set; }
        Boolean Hostile { get; set; }
        string ToString();
    }
}
