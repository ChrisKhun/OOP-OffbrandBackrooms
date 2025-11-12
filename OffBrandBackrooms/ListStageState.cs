using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class ListStageState : Command
    {
        public ListStageState() : base("LSS") { }

        override
        public Boolean Execute(CommandFunctions commandfunctions)
        {
            Boolean result = false;

            result = commandfunctions.ListStageStates();

            ClearParameters();
            return result;
        }
        
        private void ClearParameters()
        {
            Parameter0 = null;
            Parameter1 = null;
            Parameter2 = null;
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

