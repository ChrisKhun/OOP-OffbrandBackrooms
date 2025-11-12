using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class ListAllRooms : Command
    {
        public ListAllRooms() : base("LAR") { }

        override
        public Boolean Execute(CommandFunctions commandfunctions)
        {
            Boolean result = false;
            result = commandfunctions.ListAllRooms();
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

