using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class CurrentRoomDescription : Command
    {
        public CurrentRoomDescription() : base("CRD") { }

        override
        public Boolean Execute(CommandFunctions commandfunctions)
        {
            Boolean result = false;
            result = commandfunctions.CurrentRoomDescription();
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

