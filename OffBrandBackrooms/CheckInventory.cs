using OffBrandBackrooms;

namespace OffBrandBackrooms
{
  public class CheckInventory : Command
  {
    public CheckInventory() : base("checkinventory") { }

    override
    public Boolean Execute(CommandFunctions commandfunctions)
    {
        Boolean result = false;
        result = commandfunctions.CheckInventory();
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