using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class PlayerStats : Command
    {
        public PlayerStats() : base("playerstats") { }

        override
        public Boolean Execute(CommandFunctions commandfunctions)
        {
            Boolean result = false;
            result = commandfunctions.PlayerStats();
            return result;
        }
        

        override
        public Boolean Undo(CommandFunctions commandfunctions)
        {
            Boolean result = false;

            result = true;
            return result;
        }

    }
}

