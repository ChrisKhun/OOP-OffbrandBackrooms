using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class MovePlayer : Command
    {
        public MovePlayer() : base("move") { }

        override
        public Boolean Execute(CommandFunctions commandfunctions)
        {
            Boolean result = false;
            if (!string.IsNullOrEmpty(Parameter0)) 
            {
                result = commandfunctions.MovePlayer(Parameter0);
            }
        
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

