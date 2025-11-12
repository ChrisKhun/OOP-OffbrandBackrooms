using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class ListAllCommands : Command
    {
        public ListAllCommands() : base("LAC") { }

        override
        public Boolean Execute(CommandFunctions commandfunctions)
        {
            Boolean result = false;
            result = commandfunctions.ListAllCommands();
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

