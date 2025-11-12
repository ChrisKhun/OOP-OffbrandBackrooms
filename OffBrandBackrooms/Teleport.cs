using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class Teleport : Command
    {
        public Teleport() : base("teleport") { }

        override
        public Boolean Execute(CommandFunctions commandfunctions)
        {
            Boolean result = false;
            result = commandfunctions.Teleport();
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

