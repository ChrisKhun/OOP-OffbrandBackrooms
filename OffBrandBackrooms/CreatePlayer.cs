using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public class CreatePlayer : Command
    {
        public CreatePlayer() : base("CP") { }

        override
        public Boolean Execute(CommandFunctions commandfunctions)
        {
            Boolean result = false;

            if (!string.IsNullOrEmpty(Parameter0) && !string.IsNullOrEmpty(Parameter1))
            {
                result = commandfunctions.CreatePlayer(Parameter0, Parameter1);
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