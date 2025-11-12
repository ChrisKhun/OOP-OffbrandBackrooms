using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class MoveBack : Command
    {
        public MoveBack() : base("moveback") { }

        override
    public Boolean Execute(CommandFunctions commandfunctions)
    {
        Boolean result = false;
        result = commandfunctions.MoveBack();        
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
