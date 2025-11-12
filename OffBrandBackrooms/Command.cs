using OffBrandBackrooms;

namespace OffBrandBackrooms
{
    public abstract class Command
    {
        public string Name { get; private set; }
        public string? Parameter0 { set; get; }
        public string? Parameter1 { set; get; }
        public string? Parameter2 { set; get; }
        
        public Boolean HasParameter0 { get { return Parameter0 != null; } }
        public Boolean HasParameter1 { get { return Parameter1 != null; } } 
        public Boolean HasParameter2 { get { return Parameter2 != null; } }
        public Command(string name)
        {
            Name = name;
            Parameter0 = null;
            Parameter1 = null;
            Parameter2 = null;
        }

        public abstract Boolean Execute(CommandFunctions commandfunctions);
        public abstract Boolean Undo(CommandFunctions commandfunctions);

        override
        public string ToString()
        {
            return Name + (HasParameter0 ? " " + Parameter0 + (HasParameter1 ? " "             + Parameter1 + (HasParameter2 ? " " + Parameter2 : "") : "") : "");
        }
    }
}