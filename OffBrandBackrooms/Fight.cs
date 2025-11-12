using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class Fight : Command
    {
        public Fight() : base("fight") { }

        override
        public Boolean Execute(CommandFunctions commandfunctions)
        {
            Boolean result = false;

            if (!string.IsNullOrEmpty(Parameter0))
            {
                result = commandfunctions.Fight(Parameter0, Parameter1, Parameter2);
            }

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

